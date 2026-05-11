
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
    }
}
