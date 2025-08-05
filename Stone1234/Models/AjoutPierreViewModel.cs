using System.ComponentModel.DataAnnotations;

namespace Stones.Models
{
    public class AjouterPierreViewModel
    {
        [Required(ErrorMessage = "Le nom de la pierre est obligatoire")]
        public string NomPierre { get; set; }

        [Required(ErrorMessage = "La qualité est obligatoire")]
        public int IdQualite { get; set; }

        [Required(ErrorMessage = "La forme est obligatoire")]
        public int IdForme { get; set; }

        [Required(ErrorMessage = "Le carat est obligatoire")]
        public int IdCarat { get; set; }
        public decimal ValeurCarat { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        public decimal PrixVente { get; set; }

        [Required(ErrorMessage = "La quantité est obligatoire")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La quantité doit être positive")]
        public decimal Quantite { get; set; }
    }
}