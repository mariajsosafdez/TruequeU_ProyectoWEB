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
        public async Task<ListingResponseDTO> Create(Listings listing, List<string> ImageUrls)
        {
            _context.Listings.Add(listing);

            if (ImageUrls != null && ImageUrls.Any())
            {
                foreach (var url in ImageUrls)
                {
                    var newImage = new ListingImage
                    {
                        Url = url,
                        ListingID = listing.IdListing //asignamos FK
                    };

                    // se agregan al DbSet de forma independiente pq tienen su propia tabla
                    _context.ListingImages.Add(newImage);
                }
            }

            await _context.SaveChangesAsync();//guarda una única vez
            return MapToResponseDTO(listing,ImageUrls);//retorna el objeto
        }

        public async Task<List<ListingResponseDTO>> GetAll()
        {
            //usa el include como una especie de join para obtener la info del owner
            var listings = await _context.Listings
                .Include(l => l.Owner)
                .Include(l=>l.Images)
                .Where(l => l.isActive)
                .ToListAsync();

            return listings.Select(l => MapToResponseDTO(l)).ToList(); ;
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
            var listings = await _context.Listings
                .Include(l => l.Owner)
                .Include(l => l.Images)
                .Where(l => l.OwnerId == ownerId && l.isActive)
                .ToListAsync();

            return listings.Select(l => MapToResponseDTO(l)).ToList();
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
            var favs= await _context.Favorites
                .Where(f => f.ClientId == clientId)
                .Include(f => f.Listing)
                .ThenInclude(l => l!.Owner)
                .Include(f => f.Listing)
                .ThenInclude(l => l!.Images)
                .ToListAsync();
            return favs.Select(l => MapToResponseDTO(l.Listing!)).ToList();
        }

        public async Task<List<ListingResponseDTO>> GetFiltered(ListingFilterDto filters)
        {
            //Trae datos del owner (Include es como un join) y valida que está activo
            var query = _context.Listings
                .Include(l=>l.Owner)
                .Include(l=> l.Images)
                .Where(l=>l.isActive);

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

            var listings= await query.ToListAsync();//trae la lista al final con todos los filtros aplicados

            return listings.Select(l => MapToResponseDTO(l)).ToList();//mapea a responseDTO con fn auxiliar
        }

        private static ListingResponseDTO MapToResponseDTO(Listings l, List<string>? imageUrls = null)
        {
            return new ListingResponseDTO
            {
                IdListing = l.IdListing,
                Titulo = l.Titulo,
                Condicion = l.Condicion,
                Categoria = l.Categoria,
                Precio = l.Precio,
                Estado = l.Estado,
                OwnerName = l.Owner?.NombreCliente ?? "Estudiante EIA",
                PreviewImageUrl = imageUrls?.FirstOrDefault()
                    ?? l.Images?.Select(img => img.Url).FirstOrDefault()
                    ?? "placeholder.png"
            };
        }
    }
}
