using Microsoft.AspNetCore.Identity;
using TruequeU.Models.DTO;

namespace TruequeU.Interfaces
{
    public interface IAuthService
    {
        public Task<IdentityResult> Register(string email, string password, string rol);
        public Task<IdentityResult> RegisterClient(RegisterClientDTO model);

        Task<string> Login(string email, string pwd);
    }
}
