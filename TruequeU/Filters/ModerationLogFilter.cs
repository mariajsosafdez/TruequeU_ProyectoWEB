using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using TruequeU.Models;
using TruequeU.Persistence;

namespace TruequeU.Filters
{
    public class ModerationLogFilter : IAsyncActionFilter
    {
        private readonly ApplicationDbContext _context;

        public ModerationLogFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Ejecuta el endpoint primero
            var executed = await next();

            // Extrae datos del usuario del token
            var user = context.HttpContext.User;
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anónimo";
            var email = user.FindFirstValue(ClaimTypes.Email) ?? "Anónimo";
            var role = user.FindFirstValue(ClaimTypes.Role) ?? "Anónimo";

            // Arma la acción con método y ruta
            var request = context.HttpContext.Request;
            var action = $"{request.Method} {request.Path}";

            // Código de respuesta
            var resultCode = executed.HttpContext.Response.StatusCode;

            var log = new ModerationLog
            {
                LogId = Guid.NewGuid(),
                UserId = userId,
                UserEmail = email,
                UserRole = role,
                Action = action,
                ResultCode = resultCode,
                CreatedAt = DateTime.UtcNow
            };

            _context.ModerationLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}