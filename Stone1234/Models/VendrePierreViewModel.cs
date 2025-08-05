// VendrePierreViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace Stones.Models
{
    public class VendrePierreViewModel
    {
        public int Id_pierre { get; set; }
        public string Nom_pierre { get; set; }
        public string Qualite { get; set; }
        public decimal Carat { get; set; }
        public string Forme { get; set; }
        public decimal Prix_vente { get; set; }
        public decimal QuantiteDisponible { get; set; }

        [Required(ErrorMessage = "La quantité est obligatoire")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La quantité doit être positive")]
        public decimal QuantiteAVendre { get; set; }

        public decimal Total => QuantiteAVendre * Prix_vente;
    }
}