using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string? Password { get; set; }
    }
}
