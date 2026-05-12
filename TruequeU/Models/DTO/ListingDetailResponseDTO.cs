namespace TruequeU.Models.DTO
{
    public class ListingDetailResponseDTO : ListingResponseDTO
    {
        public string Descripcion { get; set; }
        public Location Ubicacion { get; set; }
        public Guid OwnerId { get; set; }
    }
}
