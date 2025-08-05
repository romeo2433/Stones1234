using System.ComponentModel.DataAnnotations;

namespace Stones.Models
{
    public class PierreStockViewModel
    {
        public int Id_pierre { get; set; }
        public string Nom_pierre { get; set; }
        public decimal Prix_vente { get; set; }
        public string Qualite { get; set; }
        public decimal Carat { get; set; }
        public string Forme { get; set; }
        public decimal Quantite_totale { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ValeurTotale => Prix_vente * Quantite_totale;
    }
    public class SearchCriteria
    {
        public string NomPierre { get; set; }
        public string Qualite { get; set; }
        public decimal? CaratMin { get; set; }
        public decimal? CaratMax { get; set; }
        public string Forme { get; set; }
        public decimal? QuantiteMin { get; set; }
        public decimal? QuantiteMax { get; set; }
    }
}