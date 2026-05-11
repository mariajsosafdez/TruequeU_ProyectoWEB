using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TruequeU.Interfaces;
using TruequeU.Models;
using TruequeU.Models.DTO;

namespace TruequeU.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(Roles = "Client")]
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
        public IActionResult Index()
        {
            return View();
        }
    }
}
