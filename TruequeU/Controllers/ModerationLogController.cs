using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TruequeU.Interfaces;

namespace TruequeU.Controllers
{
    [ApiController]
    [Route("api/moderation-logs")]
    [Authorize(Roles = "Admin")]
    public class ModerationLogController : Controller
    {
        private readonly IModerationLogService _logService;

        public ModerationLogController(IModerationLogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? role,
            [FromQuery] string? action,
            [FromQuery] int? resultCode,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var logs = await _logService.GetAll(role, action, resultCode, from, to);
            return Ok(logs);
        }

        [HttpGet("{logId}")]
        public async Task<IActionResult> GetById(Guid logId)
        {
            try
            {
                var log = await _logService.GetById(logId);
                return Ok(log);
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }

        }
    }
}