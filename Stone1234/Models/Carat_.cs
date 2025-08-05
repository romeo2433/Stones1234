using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Stones.Models
{
    public class Carat_
    {
        [Key]
        public int id_carat { get; set; }

        [Required(ErrorMessage = "La valeur en carat est obligatoire")]
        [Display(Name = "Valeur (carats)")]
        public decimal valeur { get; set; }

        // Relation avec Pierres
        public ICollection<Pierre> Pierres { get; set; }
    }
}