using TruequeU.Enums;

namespace TruequeU.DTOs
{
    public class ListingFilterDto
    {
        public string? Titulo { get; set; }
        public ListingStatus? Estado { get; set; }
        public ListingCategory? Categoria { get; set; }
        public ListingCondition? Condicion { get; set; }
        public ListingLocation? Ubicacion { get; set; }
        public float? PrecioMin { get; set; }
        public float? PrecioMax { get; set; }
    }
}
