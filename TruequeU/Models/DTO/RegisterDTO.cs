using System.ComponentModel.DataAnnotations;

namespace TruequeU.Models.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage ="La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }
        [Required(ErrorMessage = "El rol es obligatorio")]
        public string Rol { get; set; }
    }
}
