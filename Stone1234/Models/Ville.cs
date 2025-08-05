using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stones.Models
{
    [Table("villes")]
    public class Ville
    {
        [Key]
        [Column("id_villes")]
        public int Id_villes { get; set; }

        [Required(ErrorMessage = "Le nom de la ville est obligatoire")]
        [Column("nom_ville")]
        [StringLength(50)]
        [Display(Name = "Ville")]
        public string Nom_ville { get; set; }

        // Relation inverse avec Pavillon
        public ICollection<Pavillon> Pavillons { get; set; }
    }
}