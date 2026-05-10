using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruequeU.Models
{
    public enum Condition { Nuevo, Usado }
    public enum Status { Disponible, Reservado, Intercambiado }
    public enum Category { Libros, Utiles, Tecnologia, Accesorios, Otro}
    public enum Location { SedePalmas, SedeZuniga }
    public class Listings
    {
        [Key]
        public Guid IdListing { get; set; } = Guid.NewGuid();

        [Required]
        [MinLength(8)]
        [MaxLength(40)]
        public string Titulo { get; set; }

        [Required]
        [MinLength(20)]
        [MaxLength(500)]
        public string Descripcion { get; set; }
        [Required]
        public Condition Condicion { get; set; }
        public Status Estado { get; set; } = Status.Disponible;
        [Required]
        public Category Categoria { get; set; }
        public Location Ubicacion { get; set; } = Location.SedePalmas;

        [Required]
        public Guid OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public Clients? Owner { get; set; }
    }
}
