using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TruequeU.Filters;
using TruequeU.Interfaces;
using TruequeU.Models;
using TruequeU.Models.DTO;

namespace TruequeU.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(Roles = "Client")]
    [ServiceFilter(typeof(ModerationLogFilter))]
    public class ListingController : Controller
    {
        private readonly IListingService _listingService;
        public ListingController(IListingService listingService)
        {
            _listingService = listingService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ListingCreateDTO model)
        {
            var clientId = User.FindFirst("ClientId")?.Value;

            if (clientId == null) return Unauthorized("El usuario logueado no tiene rol Client");

            var newListing = new Listings
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                Condicion = model.Condicion,
                Categoria = model.Categoria,
                Ubicacion = model.Ubicacion,
                Precio = model.Precio,
                OwnerId = Guid.Parse(clientId)
            };

            var result = await _listingService.Create(newListing);

            return Ok(result);
        }
        [HttpGet]
        [AllowAnonymous] // es tipo el catálogo, lo ve aunque no esté logueado
        public async Task<IActionResult> GetAll()
        {
            var listings = await _listingService.GetAll();//el getAll va a devolver los que estén activos (ListingResponseDTO, no el obj completo)

            return Ok(listings);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var listing = await _listingService.GetById(id);

            if (listing == null)
                return NotFound("La publicación no existe o fue eliminada");

            return Ok(listing);
        }
        //como un getAll (lista) pero por propietario, para usar en perfiles
        [HttpGet("owner/{ownerId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByOwnerId(Guid ownerId)
        {
            var listings = await _listingService.GetByOwnerId(ownerId);
            return Ok(listings);
        }
        [HttpPut("changeStatus")]
        public async Task<IActionResult> ChangeStatus ([FromBody] ChangeStatusDTO entrada)
        {
            var clientId = User.FindFirst("ClientId")?.Value;//Toma ClientId del token
            if (clientId == null) return Unauthorized();

            var success=await _listingService
                .ChangeStatus(entrada.ListingId,entrada.NuevoEstado,Guid.Parse(clientId));

            if (!success)
                return BadRequest("No se pudo actualizar el estado");

            return NoContent();
        }
        [HttpPatch("{id}/softDelete")]
        public async Task<IActionResult> SoftDelete(Guid id)//id del listing a eliminar
        {
            var clientId = User.FindFirst("ClientId")?.Value;
            if (clientId == null) return Unauthorized();

            var success = await _listingService.SoftDelete(id, Guid.Parse(clientId));

            if (!success)
                return BadRequest("No se pudo eliminar la publicación");

            return NoContent();
        }

        [HttpPost("{id}/favorite")]
        public async Task<IActionResult> ToggleFavorite(Guid id)
        {
            var clientId = User.FindFirst("ClientId")?.Value;
            if (clientId == null) return Unauthorized();

            var success = await _listingService.ToggleFavorite(id, Guid.Parse(clientId));

            return success ? Ok() : BadRequest("No se pudo procesar la acción de favorito");
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites()
        {
            var clientId = User.FindFirst("ClientId")?.Value;
            if (clientId == null) return Unauthorized();

            var favorites = await _listingService.GetFavoritesByClient(Guid.Parse(clientId));
            return Ok(favorites);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
