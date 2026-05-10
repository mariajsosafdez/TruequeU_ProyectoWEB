using Microsoft.EntityFrameworkCore;
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
            var clienteExiste = GetByUserId(client.IdentityUserId);
            if (clienteExiste != null) return null;//un user solo puede hacerse cliente una vez

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Clients?> GetByUserId(string userId)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.IdentityUserId == userId);
        }
    }
}
