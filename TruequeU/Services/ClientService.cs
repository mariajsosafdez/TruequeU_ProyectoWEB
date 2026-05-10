using TruequeU.Interfaces;
using TruequeU.Persistence;

namespace TruequeU.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;
        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
