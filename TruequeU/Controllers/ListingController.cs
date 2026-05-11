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
        private readonly IClientService _clientService;
        public ListingController(IListingService listingService, IClientService clientService)
        {
            _listingService = listingService;
            _clientService = clientService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ListingCreateDTO model)
        {
            var userIdFromTk = User.FindFirstValue(ClaimTypes.NameIdentifier);// sacar el id del token (usuario logueado)
            if (string.IsNullOrEmpty(userIdFromTk)) return Unauthorized();

            var clientProfile = await _clientService.GetByUserId(userIdFromTk);//obtener el objeto cliente con el idusuario

            if (clientProfile == null)
                return Unauthorized("Este usuario aún no tiene perfil de cliente");

            var newListing = new Listings
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                Condicion = model.Condicion,
                Categoria = model.Categoria,
                Ubicacion = model.Ubicacion,
                Precio = model.Precio,
                OwnerId = clientProfile.ClientId//toma el ClientId desde el objeto de antes
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
        public IActionResult Index()
        {
            return View();
        }
    }
}
