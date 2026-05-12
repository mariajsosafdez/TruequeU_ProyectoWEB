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
            //usa el include como una especie de join para obtener la info del owner
            var listings = await _context.Listings.Include(l => l.Owner).Where(l => l.isActive).ToListAsync();

            // Mapeo a DTO compacto para dar solo la info que usan los cards
            var result = listings.Select(l => new ListingResponseDTO
            {
                IdListing = l.IdListing,
                Titulo = l.Titulo,
                Condicion = l.Condicion,
                Categoria = l.Categoria,
                Precio = l.Precio,
                Estado = l.Estado,
                OwnerName = l.Owner!.NombreCliente
            }).ToList();

            return result;
        }
        //traer un solo listing por id (Detalles)
        public async Task<ListingDetailResponseDTO?> GetById(Guid id)
        {
            return await _context.Listings
                .Include(l => l.Owner)//como si fuera un join
                .Where(l => l.IdListing == id && l.isActive)
                .Select(l => new ListingDetailResponseDTO
                {
                    IdListing = l.IdListing,
                    Titulo = l.Titulo,
                    Descripcion = l.Descripcion,
                    Condicion = l.Condicion,
                    Categoria = l.Categoria,
                    Precio = l.Precio,
                    Estado = l.Estado,
                    OwnerName = l.Owner!.NombreCliente,
                    OwnerId = l.OwnerId//Saca lo que necesita del dueño (Client que creó el listing)
                })
                .FirstOrDefaultAsync();
        }

        //traer los listings que creó un Client (owner)
        public async Task<List<ListingResponseDTO>> GetByOwnerId(Guid ownerId)//id de Client
        {
            return await _context.Listings
                .Where(l => (l.OwnerId == ownerId) && l.isActive)
                .Select(l => new ListingResponseDTO
                {
                    IdListing = l.IdListing,
                    Titulo = l.Titulo,
                    Condicion = l.Condicion,
                    Categoria = l.Categoria,
                    Precio = l.Precio,
                    Estado = l.Estado,
                    OwnerName = l.Owner!.NombreCliente
                })
                .ToListAsync();
        }
        public async Task<bool> ChangeStatus(Guid listingId, Status nuevoEstado, Guid clientId)
        {
            var listing = await _context.Listings.FindAsync(listingId);

            if (listing == null || listing.OwnerId != clientId) return false;//listing no existe o el Client no es owner
            if (listing.Estado == Status.Intercambiado) return false;//lógica de negocio, si ya intercambió no se puede devolver

            listing.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> SoftDelete(Guid id, Guid clientId)
        {
            var listing = await _context.Listings.FindAsync(id);

            if (listing == null || listing.OwnerId != clientId) return false;//existe listing y el Client logueado es dueño

            listing.isActive = false;//borrado lógico

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleFavorite(Guid listingId, Guid clientId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.ListingId == listingId && f.ClientId == clientId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
            }
            else
            {
                var newFav = new Favorites
                {
                    ListingId = listingId,
                    ClientId = clientId
                };
                await _context.Favorites.AddAsync(newFav);
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
