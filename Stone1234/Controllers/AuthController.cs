using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stones.Data;
using Stones.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Stones.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Inscription
        [HttpGet]   
        public IActionResult Inscription()
        {
            return View();
        }

        // POST: /Auth/Inscription
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscription(Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Vérifier si le numéro existe déjà
                    bool numeroExists = await _context.Utilisateur.AnyAsync(u => u.Numero == utilisateur.Numero);
                    if (numeroExists)
                    {
                        ModelState.AddModelError("Numero", "Ce numéro est déjà utilisé");
                        return View(utilisateur);
                    }

                    // Pas besoin de générer manuellement l'ID avec SERIAL en PostgreSQL
                    // L'ID sera auto-généré par la base de données
                    _context.Add(utilisateur);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Inscription réussie!";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Une erreur est survenue: {ex.Message}");
                    return View(utilisateur);
                }
            }
            return View(utilisateur);
        }


        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var utilisateur = await _context.Utilisateur
                    .FirstOrDefaultAsync(u => u.Numero == model.Numero);

                if (utilisateur != null)
                {
                    // Connexion réussie
                    return RedirectToAction("Bienvenue", new { nom = utilisateur.NomComplet });
                }

                ModelState.AddModelError(string.Empty, "Numéro incorrect");
            }

            return View(model);
        }

        // GET: /Auth/Bienvenue
        public IActionResult Bienvenue(string nom)
        {
            ViewBag.NomUtilisateur = nom;
            return View();
        }
    }
}
