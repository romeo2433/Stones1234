using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stones.Models
{
    public class MouvementStock
    {
        [Key]
        [Column("id_mvt")]
        public int Id_mvt { get; set; }

        [Required(ErrorMessage = "La date du mouvement est obligatoire")]
        [Column("date_mouvement")]
        [Display(Name = "Date du mouvement")]
        [DataType(DataType.Date)]
        public DateTime Date_mouvement { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Le type de mouvement est obligatoire")]
        [Column("type_mouvement")]
        [Display(Name = "Type de mouvement")]
        [StringLength(50)]
        public string Type_mouvement { get; set; } // "entree" ou "sortie"

        [Column("total", TypeName = "decimal(15,2)")] // Correction ici - fusion des deux annotations
        [Display(Name = "Montant total")]
        public decimal? Total { get; set; }

        // Clés étrangères
        [ForeignKey("Utilisateur")]
        [Column("id_utilisateur")]
        public int Id_utilisateur { get; set; }

        [ForeignKey("Pavillon")]
        [Column("id_pavillon")]
        public int Id_pavillon { get; set; }

        // Propriétés de navigation
        public Utilisateur Utilisateur { get; set; }
        public Pavillon Pavillon { get; set; }

        // Relation avec les détails
        public ICollection<MouvementStockDetail> MouvementStockDetails { get; set; }
    }
}