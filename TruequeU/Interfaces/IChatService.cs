using TruequeU.Models;
using TruequeU.Models.DTO;

namespace TruequeU.Interfaces
{
    public interface IChatService
    {

        Task<ChatDetailDTO> NewChat(Guid sellerId, Guid buyerId);

        Task<ChatDetailDTO> GetChatById(Guid chatId, Guid clientId);

        Task<List<ChatSummaryDTO>> GetMyChats(Guid clientId);
    }
}
