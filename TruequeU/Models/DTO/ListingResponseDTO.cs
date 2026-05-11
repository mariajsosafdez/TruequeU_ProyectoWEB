using System.ComponentModel.DataAnnotations;

namespace TruequeU.Models.DTO
{
    public class ListingResponseDTO
    {
        public Guid IdListing { get; set; }
        public string Titulo { get; set; }
        public Condition Condicion { get; set; }
        public Status Estado { get; set; }
        public Category Categoria { get; set; }
        public float Precio { get; set; }
        public string OwnerName { get; set; }
    }
}
