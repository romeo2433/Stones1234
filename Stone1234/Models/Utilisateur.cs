using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stones.Models
{
    [Table("utilisateur")]
    public class Utilisateur
    {
        [Key]
        [Column("id_utilisateur")]
        [Display(Name = "ID Utilisateur")]
        public int IdUtilisateur { get; set; }  // Type string car U001, U002 etc.

        [Required(ErrorMessage = "Le nom complet est obligatoire")]
        [Column("nom_complet")]
        [Display(Name = "Nom Complet")]
        public string NomComplet { get; set; }


        [Required(ErrorMessage = "Le numéro est obligatoire")]
        [Column("numero")]
        [Display(Name = "Numéro de téléphone")]
        [Phone(ErrorMessage = "Format de numéro invalide")]
        public string Numero { get; set; }
    }
}