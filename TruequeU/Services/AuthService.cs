using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TruequeU.Interfaces;
using TruequeU.Models;
using TruequeU.Models.DTO;
using TruequeU.Persistence;

namespace TruequeU.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<IdentityResult> Register(string email, string pw, string role)
        {
            var userExists = await _userManager.FindByEmailAsync(email);
            if (userExists != null)
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateEmail",
                    Description = "El correo ya está registrado"
                });

            var user = new IdentityUser
            {
                UserName = email,
                Email = email
            };
            var result = await _userManager.CreateAsync(user, pw);//por defecto ya cifra la contraseña
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));//si no está el rol lo crea .-.
                    //la buena práctica sería tener eso separado en endpoints
                }
                await _userManager.AddToRoleAsync(user, role); //al usuario agregale el rol :)
            }
            return result;
        }
        public async Task<IdentityResult> RegisterClient(RegisterClientDTO model)//Registrar User y Client en un solo paso
        {
            using var transaction = await _context.Database.BeginTransactionAsync();//Asegura que se creen ambos
            //si algo falla antes de completar, revierte para evitar conflictos
            //(así no queda un User con rol = Client y sin perfil de Cliente)

            var result = await Register(model.Email, model.Password, "Client");//Crea User con la otra función
            
            if (!result.Succeeded) return result;
            //Si lo pudo registrar, toma el objeto user pa poder crear ahora sí al Client
            var user = await _userManager.FindByEmailAsync(model.Email);

            var newClient = new Clients//Modelo cliente a partir del DTO
            {
                NombreCliente = model.NombreCliente,
                Carrera = model.CarreraCliente,
                IdentityUserId = user!.Id//id del User que registró
            };

            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();//Trata de guardar Client

            //si algo falla se sale antes y hace rollback (por defecto revierte los cmabios) 
            await transaction.CommitAsync();//solo cierra la transacción si hizo todo bien
            
            return IdentityResult.Success;//devuelve que todo bien
        }
        public async Task<string> Login(string email, string pwd)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.CheckPasswordAsync(user, pwd))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var client = await _context.Clients
                    .FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);
                return GetJWTToken(user, userRoles, client?.ClientId);
            }

            return null;
        }

        private string GetJWTToken(IdentityUser user, IList<string> roles, Guid? clientId)
        {
            var userRole = roles.FirstOrDefault();
            //trabajar con un rol único, no se puede ser Admin y Client a la vez

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userRole)
            };

            var authSignatureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

            if(clientId.HasValue)
                authClaims.Add(new Claim("ClientId", clientId.Value.ToString()));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    authSignatureKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
