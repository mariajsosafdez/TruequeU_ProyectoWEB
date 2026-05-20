using Microsoft.EntityFrameworkCore;
using TruequeU.DTOs;
using TruequeU.Enums;
using TruequeU.Interfaces;
using TruequeU.Models;
using TruequeU.Models.DTO;
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
        public async Task<bool> ChangeStatus(Guid listingId, ListingStatus nuevoEstado, Guid clientId)
        {
            var listing = await _context.Listings.FindAsync(listingId);

            if (listing == null || listing.OwnerId != clientId) return false;//listing no existe o el Client no es owner
            if (listing.Estado == ListingStatus.Intercambiado) return false;//lógica de negocio, si ya intercambió no se puede devolver

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
        public async Task<List<ListingResponseDTO>> GetFavoritesByClient(Guid clientId)
        {
            return await _context.Favorites
                .Where(f => f.ClientId == clientId)
                .Include(f => f.Listing)
                .ThenInclude(l => l!.Owner)
                .Select(f => new ListingResponseDTO
                {
                    IdListing = f.ListingId, //id de la tabla intermedia
                    Titulo = f.Listing!.Titulo, // acceso vía propiedad de navegación
                    Condicion = f.Listing.Condicion,
                    Categoria = f.Listing.Categoria,
                    Precio = f.Listing.Precio,
                    Estado = f.Listing.Estado,
                    OwnerName = f.Listing.Owner!.NombreCliente
                })
                .ToListAsync();
        }

        public async Task<List<ListingResponseDTO>> GetFiltered(ListingFilterDto filters)
        {
            //Trae datos del owner (Include es como un join) y valida que está activo
            var query = _context.Listings.Include(l=>l.Owner).Where(l=>l.isActive);

            //va "construyendo" el query con cada filtro
            //no trae todavía la lista sino que va agregando cada requerimiento del pedido
            if (!string.IsNullOrEmpty(filters.Titulo))
                query = query.Where(l => l.Titulo.ToLower().Contains(filters.Titulo.ToLower()));

            if (filters.Estado.HasValue)
                query = query.Where(l => l.Estado == filters.Estado.Value);

            if (filters.Categoria.HasValue)
                query = query.Where(l => l.Categoria == filters.Categoria.Value);

            if (filters.Condicion.HasValue)
                query = query.Where(l => l.Condicion == filters.Condicion.Value);

            if (filters.Ubicacion.HasValue)
                query = query.Where(l => l.Ubicacion == filters.Ubicacion.Value);

            if (filters.PrecioMin.HasValue)
                query = query.Where(l => l.Precio >= filters.PrecioMin.Value);

            if (filters.PrecioMax.HasValue)
                query = query.Where(l => l.Precio <= filters.PrecioMax.Value);

            return await query.Select(l => new ListingResponseDTO//mapea al ResponseDTO para usar cards del front
            {
                IdListing = l.IdListing,
                Titulo = l.Titulo,
                Condicion = l.Condicion,
                Categoria = l.Categoria,
                Precio = l.Precio,
                Estado = l.Estado,
                OwnerName = l.Owner!.NombreCliente
            }).ToListAsync();//trae la lista al final con todos los filtros aplicados
        }
    }
}
