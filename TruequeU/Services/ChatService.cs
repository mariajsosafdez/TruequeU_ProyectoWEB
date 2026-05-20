using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TruequeU.Interfaces;
using TruequeU.Models;
using TruequeU.Models.DTO;
using TruequeU.Persistence;

namespace TruequeU.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Método necesario para pasar de un chat a un ChatDetail (Reutilizable)
        private ChatDetailDTO MapToDetailDto(Chat chat)
        {
            return new ChatDetailDTO
            {
                ChatId = chat.ChatId,
                CreatedAt = chat.CreatedAt,
                BuyerId = chat.Buyer.ClientId,
                BuyerName = chat.Buyer.NombreCliente,
                BuyerPuntuacion = chat.Buyer.Puntuacion,
                SellerId = chat.Seller.ClientId,
                SellerName = chat.Seller.NombreCliente,
                SellerPuntuacion = chat.Seller.Puntuacion
            };
        }

        public async Task<List<ChatSummaryDTO>> GetMyChats(Guid clientId)
        {
            // Lista de chats
            var chats = await _context.Chats
           .Include(c => c.Buyer)
           .Include(c => c.Seller)
           .Where(c => c.BuyerId == clientId || c.SellerId == clientId)
           .OrderByDescending(c => c.CreatedAt)
           .ToListAsync();

            // Para cada chat, busca el último mensaje y los no leídos

            var chatIds = chats.Select(c => c.ChatId).ToList();

            var lastMessages = await _context.Messages
           .Where(m => chatIds.Contains(m.ChatId))
           .GroupBy(m => m.ChatId)
           .Select(g => g.OrderByDescending(m => m.CreatedAt).First())
           .ToListAsync();

            var unreadCounts = await _context.Messages
            .Where(m => chatIds.Contains(m.ChatId) && m.SenderId != clientId && !m.IsRead)
            .GroupBy(m => m.ChatId)
            .Select(g => new { ChatId = g.Key, Count = g.Count() })
            .ToListAsync();

            return chats.Select(chat =>
            {
                // El "otro" es el que no eres tú
                var other = chat.BuyerId == clientId ? chat.Seller : chat.Buyer;
                var lastMessage = lastMessages.FirstOrDefault(m => m.ChatId == chat.ChatId);
                var unread = unreadCounts.FirstOrDefault(u => u.ChatId == chat.ChatId);

                return new ChatSummaryDTO
                {
                    ChatId = chat.ChatId,
                    CreatedAt = chat.CreatedAt,
                    OtherParticipantId = other.ClientId,
                    OtherParticipantName = other.NombreCliente,
                    OtherParticipantPuntuacion = other.Puntuacion,
                    LastMessagePreview = lastMessage?.Content[..Math.Min(50, lastMessage.Content.Length)],
                    LastMessageAt = lastMessage?.CreatedAt,
                    UnreadCount = unread?.Count ?? 0
                };
            }).ToList();
        }

        public async Task<ChatDetailDTO> GetChatById(Guid chatId, Guid clientId)
        {

            var chat = await _context.Chats
            .Include(c => c.Buyer)
            .Include(c => c.Seller)
            .FirstOrDefaultAsync(c => c.ChatId == chatId);

            if (chat == null)
                throw new KeyNotFoundException("El chat no existe.");

            // Valida que quien pide el chat sea parte de él
            if (chat.BuyerId != clientId && chat.SellerId != clientId)
                throw new UnauthorizedAccessException("No tienes acceso a este chat.");

            return MapToDetailDto(chat);
        }

        public async Task<ChatDetailDTO> NewChat(Guid sellerId, Guid buyerId)
        {

            var sellerExist = await _context.Clients.FindAsync(sellerId);

            if (sellerExist == null)
                throw new KeyNotFoundException("El vendedor no existe.");

            if (buyerId == sellerId)
                throw new InvalidOperationException("No puedes iniciar un chat contigo mismo.");

            // Busca si ya existe un chat entre estos dos
            var existing = await _context.Chats
            .Include(c => c.Buyer)
            .Include(c => c.Seller)
            .FirstOrDefaultAsync(c => (c.BuyerId == buyerId && c.SellerId == sellerId) ||
                                (c.BuyerId == sellerId && c.SellerId == buyerId));

            if (existing != null)
                return MapToDetailDto(existing);

            // Si no existe, lo crea
            var newChat = new Chat
            {
                ChatId = Guid.NewGuid(),
                BuyerId = buyerId,
                SellerId = sellerId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Chats.Add(newChat);
            await _context.SaveChangesAsync();

            // Recarga con Include para poder mapear (Poder acceder al Buyer y al Seller)
            var created = await _context.Chats
                .Include(c => c.Buyer)
                .Include(c => c.Seller)
                .FirstAsync(c => c.ChatId == newChat.ChatId);

            return MapToDetailDto(created);
        }
    }
}
