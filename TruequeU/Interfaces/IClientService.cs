using TruequeU.Models;

namespace TruequeU.Interfaces
{
    public interface IClientService
    {
        Task<Clients?> GetByUserId(string userId);
    }
}
