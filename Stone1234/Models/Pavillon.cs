using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stones.Models
{
    [Table("pavillons")] // Spécifie le nom de la table en base de données
    public class Pavillon
    {
        [Key]
        [Column("id_pavillon")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_pavillon { get; set; }

        [Column("matricule")]
        [StringLength(50)]
        [Display(Name = "Matricule")]
        public string? Matricule { get; set; } // Nullable car pas NOT NULL en base

        [Required(ErrorMessage = "Le nom du pavillon est obligatoire")]
        [Column("nom_pavillon")]
        [StringLength(50)]
        [Display(Name = "Nom du pavillon")]
        public string Nom_pavillon { get; set; }

        // Clé étrangère vers Ville
        [ForeignKey("Ville")]
        [Column("id_villes")]
        public int Id_villes { get; set; }

        // Propriété de navigation vers Ville
        public Ville Ville { get; set; }

        // Relation inverse avec MouvementStock
        public ICollection<MouvementStock> MouvementStocks { get; set; }
    }
}