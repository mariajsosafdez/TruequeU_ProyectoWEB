using TruequeU.Models;

namespace TruequeU.Interfaces
{
    public interface IChatService
    {

        Task<Chat> NewChat(Guid SellerId, Guid BuyerId);

        Task<Chat> GetChatById(Guid chatId, Guid requesterId);

        Task<List<Chat>> GetMyChats(Guid requesterId);
    }
}
