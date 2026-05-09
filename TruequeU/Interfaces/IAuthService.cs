using Microsoft.AspNetCore.Identity;

namespace TruequeU.Interfaces
{
    public interface IAuthService
    {
        public Task<IdentityResult> Register(string email, string password, string rol);
    }
}
