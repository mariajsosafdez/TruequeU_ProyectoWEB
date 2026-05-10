using TruequeU.Models;

namespace TruequeU.Interfaces
{
    public interface IMessageService
    {
        Task<List<Message>> GetMessagesByChat(Guid chatId, Guid requesterId);

        Task<Message> SendMessage(Guid chatId, Guid senderId, string content);
    }
}
