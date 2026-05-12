
using TruequeU.Models;
using TruequeU.Models.DTO;

namespace TruequeU.Interfaces
{
    public interface IListingService
    {
        Task<Listings> Create(Listings listing);
        Task<List<ListingResponseDTO>> GetAll();
        Task<ListingDetailResponseDTO?> GetById(Guid id);
        Task<List<ListingResponseDTO>> GetByOwnerId(Guid ownerId);
        Task<bool> ChangeStatus (Guid listingId, Status newEstado, Guid clientId);
        Task<bool> SoftDelete(Guid id, Guid clientId);

        //incluye tmb lógica de favorites
        Task<bool> ToggleFavorite(Guid listingId, Guid clientId);
        Task<List<ListingResponseDTO>> GetFavoritesByClient(Guid clientId);
    }
}
