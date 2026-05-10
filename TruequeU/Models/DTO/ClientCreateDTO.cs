using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruequeU.Models.DTO
{
    public class ClientCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        public string NombreCliente { get; set; }
        public string CarreraCliente { get; set; }
    }
}
