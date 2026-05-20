using TruequeU.Enums;

namespace TruequeU.Models.DTO
{
    public class ListingResponseDTO
    {
        public Guid IdListing { get; set; }
        public string Titulo { get; set; }
        public ListingCondition Condicion { get; set; }
        public ListingStatus Estado { get; set; }
        public ListingCategory Categoria { get; set; } 
        public float Precio { get; set; }
        public string OwnerName { get; set; }
        // El card solo usa la primer img para mostrar de portada
        public string? PreviewImageUrl { get; set; }
    }
}
