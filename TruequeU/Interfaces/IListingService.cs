
using TruequeU.Models;
using TruequeU.Models.DTO;

namespace TruequeU.Interfaces
{
    public interface IListingService
    {
        Task<Listings> Create(Listings listing);
        Task<List<ListingResponseDTO>> GetAll();
    }
}
