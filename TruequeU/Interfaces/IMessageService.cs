using TruequeU.Models;
using TruequeU.Models.DTO;

namespace TruequeU.Interfaces
{
    public interface IMessageService
    {
        Task<List<MessageResponseDTO>> GetMessagesByChat(Guid chatId, Guid requesterId);

        Task<MessageResponseDTO> SendMessage(Guid chatId, Guid senderId, string content);
    }
}
