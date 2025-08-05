using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stones.Models
{
    [Table("pierres")]
    public class Pierre
    {
        [Key]
        [Column("id_pierre")]
        public int Id_pierre { get; set; }

        [Required]
        [Column("nom_pierre")]
        public string Nom_pierre { get; set; }

        [Required]
        [Column("prix_vente", TypeName = "decimal(15,2)")]
        public decimal Prix_vente { get; set; }

        [Column("id_qua")]
        public int Id_qua { get; set; }

        [Column("id_carat")]
        public int Id_carat { get; set; }

        [Column("id_forme")]
        public int Id_forme { get; set; }

        [ForeignKey("Id_qua")]
        public Qualite Qualite { get; set; }

        [ForeignKey("Id_carat")]
        public Carat_ Carat { get; set; }

        [ForeignKey("Id_forme")]
        public Forme Forme { get; set; }

        public ICollection<MouvementStockDetail> MouvementStockDetails { get; set; }
    }
}