using TruequeU.Models;

namespace TruequeU.Interfaces
{
    public interface IChatService
    {

        Task<Chat> NewChat(Guid sellerId, Guid buyerId);

        Task<Chat> GetChatById(Guid chatId, Guid clientId);

        Task<List<Chat>> GetMyChats(Guid clientId);
    }
}
