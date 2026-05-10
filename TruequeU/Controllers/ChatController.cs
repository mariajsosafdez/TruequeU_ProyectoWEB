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
        public async Task<IActionResult> newChat(Guid SellerId)
        {
            try
            {
                var BuyerId = GetCurrentUserId();
                var newChat = await _chatService.NewChat(SellerId, BuyerId);
                return Ok(newChat);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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
