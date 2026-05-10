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
    [Authorize(Roles ="Client")]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientCreateDTO model)
        {
            var userIdFromTk = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //saca el IdentityUserId del token de Authorization (tkn de usuario que devuelve el endpoint de Login)
            // no tiene que pedirlo en el DTO, más sencillo y evita problemas de seguridad

            if (string.IsNullOrEmpty(userIdFromTk)) return Unauthorized();//Tkn malito

            var newClient = new Clients
            {
                NombreCliente = model.NombreCliente,
                Carrera = model.CarreraCliente,
                IdentityUserId = userIdFromTk
            };

            var created = await _clientService.Create(newClient);
            if (created == null) return Conflict("Este usuario ya tiene un perfil de cliente");
            return Ok(created);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
