using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TruequeU.Interfaces;

namespace TruequeU.Controllers
{
    [ApiController]
    [Route("api/chats/{chatId}/messages")]
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }


        private Guid GetCurrentUserId() =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetMessages(Guid chatId)
        {
            try
            {
                var requesterId = GetCurrentUserId();
                var messages = await _messageService.GetMessagesByChat(chatId, requesterId);
                return Ok(messages);
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
 
        [HttpPost]
        public async Task<IActionResult> SendMessage(Guid chatId, [FromBody] string content)
        {
            try
            {
                var senderId = GetCurrentUserId();
                var message = await _messageService.SendMessage(chatId, senderId, content);
                return CreatedAtAction(nameof(GetMessages), new { chatId }, message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
