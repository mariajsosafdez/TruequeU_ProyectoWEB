using Microsoft.AspNetCore.Identity;
using TruequeU.Interfaces;

namespace TruequeU.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> Register(string email, string pw, string role)
        {
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
    }
}
