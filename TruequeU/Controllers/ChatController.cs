using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TruequeU.Interfaces;
using TruequeU.Models;
using TruequeU.Models.DTO;

namespace TruequeU.Controllers
{
    [ApiController]
    [Route("api/chats")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        private Guid GetCurrentUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("{SellerId}")]
        public async Task<IActionResult> NewChat(Guid sellerId)
        {
            try
            {
                var buyerId = GetCurrentUserId();
                var newChat = await _chatService.NewChat(sellerId, buyerId);
                return Ok(newChat);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChatById(Guid chatId)
        {
            try
            {
                var requesterId = GetCurrentUserId();
                var chat = await _chatService.GetChatById(chatId, requesterId);
                return Ok(chat);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpGet("myChats")]
        public async Task<IActionResult> GetMyChats()
        {
            var userId = GetCurrentUserId();
            var chats = await _chatService.GetMyChats(userId);
            return Ok(chats);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
