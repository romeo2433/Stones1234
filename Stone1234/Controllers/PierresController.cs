using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Stones.Data;
using Stones.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stones.Controllers
{
    public class PierresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PierresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Stock(SearchCriteria search)
        {
            // Requête de base avec LEFT JOIN pour inclure toutes les pierres même sans mouvement
            var baseQuery = @"
    SELECT 
        p.id_pierre AS Id_pierre,
        p.nom_pierre AS Nom_pierre,
        p.prix_vente AS Prix_vente,
        q.nom_qualite AS Qualite,
        c.valeur AS Carat,
        f.nom_forme AS Forme,
        COALESCE(SUM(CASE WHEN ms.type_mouvement = 'Entrée' THEN msd.quantite ELSE 0 END), 0)
        - COALESCE(SUM(CASE WHEN ms.type_mouvement = 'Sortie' THEN msd.quantite ELSE 0 END), 0)
        AS Quantite_totale
    FROM pierres p
    JOIN qualite q ON p.id_qua = q.id_qua
    JOIN carat_ c ON p.id_carat = c.id_carat
    JOIN forme f ON p.id_forme = f.id_forme
    LEFT JOIN mouvement_stock_detail msd ON p.id_pierre = msd.id_pierre
    LEFT JOIN mouvement_stock ms ON ms.id_mvt = msd.id_mvt
";

            var whereClauses = new List<string>();
            var havingClauses = new List<string>();
            var parameters = new List<NpgsqlParameter>();
            var paramIndex = 0;

            // Filtres WHERE (pour les colonnes de table)
            if (!string.IsNullOrEmpty(search.NomPierre))
            {
                whereClauses.Add($"p.nom_pierre ILIKE @p{paramIndex}");
                parameters.Add(new NpgsqlParameter($"@p{paramIndex}", $"%{search.NomPierre}%"));
                paramIndex++;
            }

            if (!string.IsNullOrEmpty(search.Qualite))
            {
                whereClauses.Add($"q.nom_qualite = @p{paramIndex}");
                parameters.Add(new NpgsqlParameter($"@p{paramIndex}", search.Qualite));
                paramIndex++;
            }

            if (search.CaratMin.HasValue)
            {
                whereClauses.Add($"c.valeur >= @p{paramIndex}");
                parameters.Add(new NpgsqlParameter($"@p{paramIndex}", search.CaratMin.Value));
                paramIndex++;
            }

            if (search.CaratMax.HasValue)
            {
                whereClauses.Add($"c.valeur <= @p{paramIndex}");
                parameters.Add(new NpgsqlParameter($"@p{paramIndex}", search.CaratMax.Value));
                paramIndex++;
            }

            if (!string.IsNullOrEmpty(search.Forme))
            {
                whereClauses.Add($"f.nom_forme = @p{paramIndex}");
                parameters.Add(new NpgsqlParameter($"@p{paramIndex}", search.Forme));
                paramIndex++;
            }

            // Filtres HAVING (pour les agrégations)
            if (search.QuantiteMin.HasValue)
            {
                havingClauses.Add($"COALESCE(SUM(msd.quantite), 0) >= @p{paramIndex}");
                parameters.Add(new NpgsqlParameter($"@p{paramIndex}", search.QuantiteMin.Value));
                paramIndex++;
            }

            if (search.QuantiteMax.HasValue)
            {
                havingClauses.Add($"COALESCE(SUM(msd.quantite), 0) <= @p{paramIndex}");
                parameters.Add(new NpgsqlParameter($"@p{paramIndex}", search.QuantiteMax.Value));
                paramIndex++;
            }

            // Construction de la requête finale
            if (whereClauses.Any())
            {
                baseQuery += "WHERE " + string.Join(" AND ", whereClauses) + "\n";
            }

            baseQuery += "GROUP BY p.id_pierre, p.nom_pierre, p.prix_vente, q.nom_qualite, c.valeur, f.nom_forme\n";

            if (havingClauses.Any())
            {
                baseQuery += "HAVING " + string.Join(" AND ", havingClauses) + "\n";
            }

            // Tri par défaut
            baseQuery += "ORDER BY p.nom_pierre ASC";

            try
            {
                var query = _context.PierreStockViewModels.FromSqlRaw(baseQuery, parameters.ToArray());
                var pierresStock = await query.AsNoTracking().ToListAsync();

                // Préparation des listes pour les filtres
                ViewBag.Qualites = await _context.Qualite.Select(q => q.Nom_qualite).Distinct().ToListAsync();
                ViewBag.Formes = await _context.Forme.Select(f => f.nom_forme).Distinct().ToListAsync();
                ViewBag.SearchCriteria = search;

                return View(pierresStock);
            }
            catch (NpgsqlException ex)
            {
                // Log l'erreur (à implémenter)
                ModelState.AddModelError("", "Une erreur est survenue lors de la récupération des données.");
                return View(new List<PierreStockViewModel>());
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Récupérer la pierre
            var pierre = await _context.Pierres.FirstOrDefaultAsync(p => p.Id_pierre == id);

            if (pierre == null)
            {
                TempData["error"] = "Pierre introuvable.";
                return RedirectToAction("Stock");
            }

            // Récupérer les mouvements stock liés à cette pierre via les détails
            var mouvementsIds = await _context.MouvementStockDetail
                .Where(m => m.Id_pierre == id)
                .Select(m => m.Id_mvt)
                .Distinct()
                .ToListAsync();

            // Récupérer les mouvements parent correspondants
            var mouvements = await _context.MouvementStock
                .Where(mvt => mouvementsIds.Contains(mvt.Id_mvt))
                .ToListAsync();

            // Supprimer tous les détails liés à la pierre
            var mouvementsDetails = _context.MouvementStockDetail.Where(m => m.Id_pierre == id);
            _context.MouvementStockDetail.RemoveRange(mouvementsDetails);

            // Supprimer les mouvements récupérés
            _context.MouvementStock.RemoveRange(mouvements);

            // Supprimer la pierre
            _context.Pierres.Remove(pierre);

            await _context.SaveChangesAsync();

            TempData["success"] = "Pierre et ses mouvements supprimés avec succès.";
            return RedirectToAction("Stock");
        }
        [HttpGet]
        public async Task<IActionResult> Modifier(int id)
        {
            var pierre = await _context.Pierres.FindAsync(id);
            if (pierre == null)
            {
                TempData["error"] = "Pierre introuvable.";
                return RedirectToAction("Stock");
            }

            await ChargerViewBagsAsync();
            return View(pierre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modifier(Pierre pierre)
        {
            if (!ModelState.IsValid)
            {
                await ChargerViewBagsAsync();
                return View(pierre);
            }

            var caratExists = await _context.Carat_.AnyAsync(c => c.id_carat == pierre.Id_carat);
            if (!caratExists)
            {
                ModelState.AddModelError("Id_carat", "Le carat sélectionné n'existe pas");
                await ChargerViewBagsAsync();
                return View(pierre);
            }

            var pierreExistante = await _context.Pierres.FindAsync(pierre.Id_pierre);
            if (pierreExistante == null)
            {
                TempData["error"] = "Pierre introuvable.";
                return RedirectToAction("Stock");
            }

            pierreExistante.Nom_pierre = pierre.Nom_pierre;
            pierreExistante.Prix_vente = pierre.Prix_vente;
            pierreExistante.Id_qua = pierre.Id_qua;
            pierreExistante.Id_carat = pierre.Id_carat;
            pierreExistante.Id_forme = pierre.Id_forme;

            await _context.SaveChangesAsync();

            TempData["success"] = "Pierre modifiée avec succès.";
            return RedirectToAction("Stock");
        }

        private async Task ChargerViewBagsAsync()
        {
            ViewBag.Qualites = await _context.Qualite.ToListAsync();
            ViewBag.Carats = await _context.Carat_.ToListAsync();
            ViewBag.Formes = await _context.Forme.ToListAsync();
        }






    }
}