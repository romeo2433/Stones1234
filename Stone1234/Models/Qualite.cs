using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stones.Models
{
    // Models/Qualite.cs (renommez le fichier sans accent)
    [Table("qualite")] // Sans accent dans le nom de table
                  
    public class Qualite
    {
        [Key]
        [Column("id_qua")]
        public int Id_qua { get; set; }

        [Required]
        [Column("nom_qualite")]
        public string Nom_qualite { get; set; }

        public ICollection<Pierre> Pierres { get; set; }
    }
}