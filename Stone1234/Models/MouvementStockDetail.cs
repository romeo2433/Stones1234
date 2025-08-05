using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stones.Models
{
    public class MouvementStockDetail
    {
        [Key]
        [Column("id_mvtdetail")]
        public int Id_mvtdetail { get; set; }

        [Required(ErrorMessage = "La quantité est obligatoire")]
        [Column("quantite", TypeName = "decimal(15,2)")] // Correction ici
        [Display(Name = "Quantité")]
        public decimal Quantite { get; set; }

        // Clés étrangères
        [ForeignKey("Pierre")]
        [Column("id_pierre")]
        public int Id_pierre { get; set; }

        [ForeignKey("MouvementStock")]
        [Column("id_mvt")]
        public int Id_mvt { get; set; }

        // Propriétés de navigation
        public Pierre Pierre { get; set; }
        public MouvementStock MouvementStock { get; set; }

        [Column("prix_unitaire", TypeName = "decimal(15,2)")]
        [Display(Name = "Prix unitaire")]
        public decimal Prix_unitaire { get; set; }
    }
}