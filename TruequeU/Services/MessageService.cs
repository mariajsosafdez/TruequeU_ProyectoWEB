using Microsoft.EntityFrameworkCore;
using TruequeU.Interfaces;
using TruequeU.Models;
using TruequeU.Persistence;

namespace TruequeU.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetMessagesByChat(Guid chatId, Guid requesterId)
        {
            var existschat = await _context.Chats.FirstOrDefaultAsync(c => c.ChatId == chatId);

            if (existschat == null)
                throw new KeyNotFoundException("El chat no existe.");

            // Si no es ni buyer ni seller, lanza excepcion
            if (existschat.BuyerId != requesterId && existschat.SellerId != requesterId)
                throw new UnauthorizedAccessException("No tienes acceso a este chat.");

            // Marca como leídos los mensajes que no mandó el requester
            var unread = await _context.Messages
                .Where(m => m.ChatId == chatId && m.SenderId != requesterId && !m.IsRead)
                .ToListAsync();

            foreach (var message in unread)
                message.IsRead = true;

            await _context.SaveChangesAsync();

            // Devuelve todos los mensajes ordenados del más antiguo al más reciente
            return await _context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<Message> SendMessage(Guid chatId, Guid senderId, string content)
        {
            // Valida que el chat exista
            var existsChat = await _context.Chats
                .FirstOrDefaultAsync(c => c.ChatId == chatId);

            if (existsChat == null)
                throw new KeyNotFoundException("El chat no existe.");

            // Valida que quien envía sea participante
            if (existsChat.BuyerId != senderId && existsChat.SellerId != senderId)
                throw new UnauthorizedAccessException("No puedes enviar mensajes en este chat.");

            // Valida que el mensaje no esté vacío
            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException("El mensaje no puede estar vacío.");

            var newMessage = new Message
            {
                MessageId = Guid.NewGuid(),
                ChatId = chatId,
                SenderId = senderId,
                Content = content.Trim(),
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            return newMessage;
        }
    }
}

