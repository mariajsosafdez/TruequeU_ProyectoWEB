using Microsoft.EntityFrameworkCore;
using TruequeU.Interfaces;
using TruequeU.Models;
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
        //public async Task<ChatDetailDto> GetOrCreateAsync(Guid buyerId, Guid sellerId)
        //{
        //    // Busca si ya existe un chat entre estos dos
        //    var existing = await _context.Chats
        //        .Include(c => c.Buyer)
        //        .Include(c => c.Seller)
        //        .Include(c => c.Messages)
        //        .FirstOrDefaultAsync(c =>
        //            c.BuyerId == buyerId && c.SellerId == sellerId);

        //    if (existing is not null)
        //        return MapToDetailDto(existing);   // Devuelve el que ya existe

        //    // Si no existe, lo crea
        //    var chat = new Chat
        //    {
        //        ChatId = Guid.NewGuid(),
        //        BuyerId = buyerId,
        //        SellerId = sellerId,
        //        CreatedAt = DateTime.UtcNow
        //    };

        //    _context.Chats.Add(chat);
        //    await _context.SaveChangesAsync();

        //    return MapToDetailDto(chat);
        //}
        public async Task<List<Chat>> GetMyChats(Guid clientId)
        {
            return await _context.Chats
            .Include(c => c.Buyer)
            .Include(c => c.Seller)
            .Where(c => c.BuyerId == clientId || c.SellerId == clientId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
        }

        public async Task<Chat> GetChatById(Guid chatId, Guid userId)
        {
            var chat = await _context.Chats
            .Include(c => c.Buyer)
            .Include(c => c.Seller)
            .FirstOrDefaultAsync(c => c.ChatId == chatId);

            if (chat == null)
                throw new KeyNotFoundException("El chat no existe.");

            // Valida que quien pide el chat sea parte de él
            if (chat.BuyerId != userId && chat.SellerId != userId)
                throw new UnauthorizedAccessException("No tienes acceso a este chat.");

            return chat;
        }

        public async Task<Chat> NewChat(Guid sellerId, Guid buyerId)
        {
            var sellerExist = await _context.Clients.FindAsync(sellerId);

            if (!sellerExist)
                throw new KeyNotFoundException("El vendedor no existe.");

            if (buyerId == sellerId)
                throw new InvalidOperationException("No puedes iniciar un chat contigo mismo.");

            // Busca si ya existe un chat entre estos dos
            var existing = await _context.Chats
                .Include(c => c.Buyer)
                .Include(c => c.Seller)
                .FirstOrDefaultAsync(c =>
                    c.BuyerId == buyerId && c.SellerId == sellerId);

            if (existing != null)
                return existing;

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

            return newChat;
        }
    }
}
