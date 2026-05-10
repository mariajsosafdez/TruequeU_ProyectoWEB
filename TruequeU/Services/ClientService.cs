using TruequeU.Interfaces;
using TruequeU.Models;
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

        public async Task<Clients> Create(Clients client)
        {
            var clienteExiste = _context.Clients.FirstOrDefault(e => e.IdentityUserId==client.IdentityUserId);
            if (clienteExiste != null) return null;//un user solo puede hacerse cliente una vez

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }
    }
}
