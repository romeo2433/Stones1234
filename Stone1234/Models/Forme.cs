using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stones.Models
{
    public class Forme
    {
        [Key]
        public int id_forme { get; set; }

        [Required(ErrorMessage = "Le nom de la forme est obligatoire")]
        [Display(Name = "Forme")]
        public string nom_forme { get; set; }

        // Relation avec Pierres
        public ICollection<Pierre> Pierres { get; set; }
    }
}