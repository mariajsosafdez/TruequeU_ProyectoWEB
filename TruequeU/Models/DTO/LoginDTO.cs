using System.ComponentModel.DataAnnotations;

namespace TruequeU.Models.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El email está vacío")]
        [EmailAddress(ErrorMessage = "Formato inválido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña está vacía")]
        public string Password { get; set; }
    }
}
