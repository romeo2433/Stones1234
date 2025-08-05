using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stones.Data;
using Stones.Models;

namespace Stones.Controllers
{
    public class AjoutPierreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AjoutPierreController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Qualites = new SelectList(await _context.Qualite.ToListAsync(), "Id_qua", "Nom_qualite");
            ViewBag.Formes = new SelectList(await _context.Forme.ToListAsync(), "id_forme", "nom_forme"); // minuscules


            return View("../Pierres/Ajouter");
        }

        [HttpPost]
        public async Task<IActionResult> Index(AjouterPierreViewModel model)
        {
            // Toujours réinitialiser les ViewBag
            ViewBag.Qualites = new SelectList(await _context.Qualite.ToListAsync(), "Id_qua", "Nom_qualite", model.IdQualite);
            ViewBag.Formes = new SelectList(await _context.Forme.ToListAsync(), "id_forme", "nom_forme", model.IdForme); // minuscules
            ViewBag.Carats = new SelectList(await _context.Carat_.ToListAsync(), "id_carat", "valeur", model.IdCarat); // minuscules

            if (!ModelState.IsValid)
            {
                return View("../Pierres/Ajouter", model);
            }

            // Idem dans tous les autres return View(model)




            // Vérifie si la pierre existe déjà (même nom, forme, qualité, carat)
            var pierreExistante = await _context.Pierres.FirstOrDefaultAsync(p =>
                p.Nom_pierre.ToLower() == model.NomPierre.ToLower() &&
                p.Id_qua == model.IdQualite &&
                p.Id_forme == model.IdForme &&
               _context.Carat_.Any(c => c.valeur == model.ValeurCarat));

            if (pierreExistante != null)
            {
                ModelState.AddModelError("", "Une pierre identique existe déjà.");
                ViewBag.Qualites = new SelectList(await _context.Qualite.ToListAsync(), "Id_qua", "Nom_qualite", model.IdQualite);
                ViewBag.Formes = new SelectList(await _context.Forme.ToListAsync(), "Id_forme", "Nom_forme", model.IdForme);
                ViewBag.Carats = new SelectList(await _context.Carat_.ToListAsync(), "Id_carat", "Valeur", model.IdCarat);

                return View("../Pierres/Ajouter", model);
            }
            var carat = await _context.Carat_.FirstOrDefaultAsync(c => c.valeur == model.ValeurCarat);
            if (carat == null)
            {
                carat = new Carat_ { valeur = model.ValeurCarat };
                _context.Carat_.Add(carat);
                await _context.SaveChangesAsync();
            }


            // Ajoute la nouvelle pierre
            var nouvellePierre = new Pierre
            {
                Nom_pierre = model.NomPierre,
                Id_qua = model.IdQualite,
                Id_forme = model.IdForme,
                Id_carat = carat.id_carat,
                Prix_vente = model.PrixVente
            };

            _context.Pierres.Add(nouvellePierre);
            await _context.SaveChangesAsync();

            decimal total = model.Quantite * model.PrixVente;

            // Crée un mouvement de stock (Entrée)
            var mouvement = new MouvementStock
            {
                Date_mouvement = DateTime.UtcNow,
                Type_mouvement = "Entrée",
                Total = total,
                Id_utilisateur = 1, // à remplacer par l'utilisateur réel
                Id_pavillon = 1     // à adapter selon la logique métier
            };

            _context.MouvementStock.Add(mouvement);
            await _context.SaveChangesAsync();

            // Détail du mouvement
            var detail = new MouvementStockDetail
            {
                Id_pierre = nouvellePierre.Id_pierre,
                Quantite = model.Quantite,
                Prix_unitaire = model.PrixVente,
                Id_mvt = mouvement.Id_mvt
            };

            _context.MouvementStockDetail.Add(detail);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Pierre ajoutée avec succès.";
            return RedirectToAction("Index");
        }
    }
}
