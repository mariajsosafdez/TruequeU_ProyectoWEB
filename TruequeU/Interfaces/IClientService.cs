using TruequeU.Models;

namespace TruequeU.Interfaces
{
    public interface IClientService
    {
        Task<Clients> Create(Clients client);
        Task<Clients?> GetByUserId(string userId);
    }
}
