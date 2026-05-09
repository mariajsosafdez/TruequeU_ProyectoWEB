using System.ComponentModel.DataAnnotations;

namespace TruequeU.Models.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        public string email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage ="La contraseña debe tener al menos 6 caracteres")]
        public string password { get; set; }
        [Required(ErrorMessage = "El rol es obligatorio")]
        public string rol { get; set; }
    }
}
