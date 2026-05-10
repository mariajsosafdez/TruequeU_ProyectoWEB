using TruequeU.Interfaces;
using TruequeU.Persistence;

namespace TruequeU.Services
{
    public class ListingService : IListingService
    {
        private readonly ApplicationDbContext _context;
        public ListingService(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
