using TruequeU.Interfaces;
using TruequeU.Persistence;
using TruequeU.Models;
using TruequeU.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace TruequeU.Services
{
    public class ListingService : IListingService
    {
        private readonly ApplicationDbContext _context;
        public ListingService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Listings> Create(Listings listing)
        {
            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();
            return listing;
        }

        public async Task<List<ListingResponseDTO>> GetAll()
        {
            var listings = await _context.Listings.Where(l => l.isActive).ToListAsync();

            // Mapeo a DTO compacto para dar solo la info que usan los cards
            var result = listings.Select(l => new ListingResponseDTO
            {
                IdListing = l.IdListing,
                Titulo = l.Titulo,
                Condicion = l.Condicion,
                Categoria = l.Categoria,
                Precio = l.Precio,
                Estado = l.Estado,
                OwnerName = l.Owner?.NombreCliente ?? "Usuario TruequeU"
            }).ToList();//por ahora no es capaz de traer nombreOwner, queda pendiente

            return result;
        }
    }
}
