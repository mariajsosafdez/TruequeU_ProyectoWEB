
using TruequeU.Models;

namespace TruequeU.Interfaces
{
    public interface IListingService
    {
        Task<Listings> Create(Listings listing);
    }
}
