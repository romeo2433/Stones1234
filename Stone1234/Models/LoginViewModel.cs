using System.ComponentModel.DataAnnotations;

namespace Stones.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Le numéro est obligatoire")]
        [Display(Name = "Numéro de téléphone")]
        [Phone(ErrorMessage = "Format invalide")]
        public string Numero { get; set; }
    }
}