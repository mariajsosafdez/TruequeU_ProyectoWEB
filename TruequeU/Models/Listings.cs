using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TruequeU.Enums;

namespace TruequeU.Models
{
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
        public ListingCondition Condicion { get; set; }
        public ListingStatus Estado { get; set; } = ListingStatus.Disponible;
        [Required]
        public ListingCategory Categoria { get; set; }
        [Required]
        public float Precio { get; set; }
        public ListingLocation Ubicacion { get; set; } = ListingLocation.SedePalmas;
        public bool isActive { get; set; } = true;

        [Required]
        public Guid OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public Clients? Owner { get; set; }
    }
}
