using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruequeU.Models
{
    public class Clients
    {
        [Key]
        public Guid ClientId { get; set; } = Guid.NewGuid();
        [Required]
        public string NombreCliente { get; set; }
        public string Carrera { get; set; }
        public float Puntuacion { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        [Required]
        public string IdentityUserId { get; set; } = string.Empty;
        [ForeignKey("IdentityUserId")]
        public IdentityUser? IdentityUser { get; set; }
    }
}
