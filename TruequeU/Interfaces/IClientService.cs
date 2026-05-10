using TruequeU.Models;

namespace TruequeU.Interfaces
{
    public interface IClientService
    {
        Task<Clients> Create(Clients client);
    }
}
