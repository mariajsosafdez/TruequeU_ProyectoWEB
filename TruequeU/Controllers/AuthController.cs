using Microsoft.AspNetCore.Mvc;
using TruequeU.Interfaces;
using TruequeU.Models.DTO;

namespace TruequeU.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var result = await _authService.Register(model.email, model.password, model.rol);

            if (result.Succeeded)
                return Ok(new { message = $"Usuario {model.email} creado correctamente" });

            return BadRequest();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
