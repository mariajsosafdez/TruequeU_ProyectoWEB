using TruequeU.Enums;

namespace TruequeU.Models.DTO
{
    public class ListingDetailResponseDTO : ListingResponseDTO
    {
        public string Descripcion { get; set; }
        public ListingLocation Ubicacion { get; set; }
        public Guid OwnerId { get; set; }
    }
}
