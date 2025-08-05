using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stones.Data;
using Stones.Models;
using Microsoft.Extensions.Logging;

namespace Stones.Controllers
{
    public class VendrePierreController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VendrePierreController> _logger;

        public VendrePierreController(ApplicationDbContext context, ILogger<VendrePierreController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Vendre(int id)
        {
            var pierre = await _context.Pierres
                .Include(p => p.Qualite)
                .Include(p => p.Forme)
                .Include(p => p.Carat)
                .FirstOrDefaultAsync(p => p.Id_pierre == id);

            if (pierre == null)
                return NotFound();

            // Entrées = somme des quantités pour les entrées
            var qteEntree = await _context.MouvementStockDetail
                .Where(m => m.Id_pierre == id && m.MouvementStock.Type_mouvement == "Entrée")
                .SumAsync(m => m.Quantite);

            // Sorties = somme des quantités pour les sorties
            var qteSortie = await _context.MouvementStockDetail
                .Where(m => m.Id_pierre == id && m.MouvementStock.Type_mouvement == "Sortie")
                .SumAsync(m => m.Quantite);

            // Calcul final : disponible = entrée - sortie
            var dispo = qteEntree - qteSortie;


            var model = new VendrePierreViewModel
            {
                Id_pierre = pierre.Id_pierre,
                Nom_pierre = pierre.Nom_pierre,
                Qualite = pierre.Qualite?.Nom_qualite,
                Carat = pierre.Carat?.valeur ?? 0,
                Forme = pierre.Forme?.nom_forme,
                Prix_vente = pierre.Prix_vente,
                QuantiteDisponible = dispo
            };

            // on s'appuie sur la convention : la vue s'appelle "Vendre" et est dans Views/VendrePierre
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Vendre(VendrePierreViewModel model)
        {
            _logger.LogInformation("Début de l'action POST Vendre");
            _logger.LogInformation($"Model reçu: {System.Text.Json.JsonSerializer.Serialize(model)}");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalide");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"Erreur: {error.ErrorMessage}");
                }
                return View(model);
            }

            _logger.LogInformation("Modèle valide, vérification du stock...");

            var quantiteTotaleEntree = await _context.MouvementStockDetail
                .Where(m => m.Id_pierre == model.Id_pierre && m.MouvementStock.Type_mouvement == "Entrée")
                .SumAsync(m => m.Quantite);

            var quantiteTotaleSortie = await _context.MouvementStockDetail
                .Where(m => m.Id_pierre == model.Id_pierre && m.MouvementStock.Type_mouvement == "Sortie")
                .SumAsync(m => m.Quantite);

            var quantiteDisponible = quantiteTotaleEntree - quantiteTotaleSortie;
            _logger.LogInformation("Quantité disponible : {quantiteDisponible}", quantiteDisponible);

            if (model.QuantiteAVendre > quantiteDisponible)
            {
                _logger.LogWarning("Quantité demandée ({QuantiteAVendre}) > stock disponible ({quantiteDisponible})",
                    model.QuantiteAVendre, quantiteDisponible);

                ModelState.AddModelError("QuantiteAVendre", "La quantité demandée n'est pas disponible en stock");
                model.QuantiteDisponible = quantiteDisponible;
                return View("Vendre", model);
            }

            try
            {
                var mouvement = new MouvementStock
                {
                    Date_mouvement = DateTime.UtcNow,
                    Type_mouvement = "Sortie",
                    Total = model.Total,
                    Id_utilisateur = 1, // À remplacer par l'utilisateur connecté
                    Id_pavillon = 1
                };

                _context.MouvementStock.Add(mouvement);
                await _context.SaveChangesAsync();

                _logger.LogInformation("MouvementStock inséré avec ID {Id}", mouvement.Id_mvt);
                

                var detail = new MouvementStockDetail
                {
                    Id_pierre = model.Id_pierre,
                    Quantite = model.QuantiteAVendre,
                    Prix_unitaire = model.Prix_vente,
                    Id_mvt = mouvement.Id_mvt
                };

                _context.MouvementStockDetail.Add(detail);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Détail du mouvement ajouté pour la pierre ID {Id_pierre}", model.Id_pierre);

                TempData["SuccessMessage"] = "Vente enregistrée avec succès.";
                return RedirectToAction("Stock", "Pierres"); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'enregistrement de la vente");
                TempData["ErrorMessage"] = "Une erreur est survenue lors de l'enregistrement de la vente.";
                return View(model);
            }
        }

    }
}
