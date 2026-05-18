using Microsoft.EntityFrameworkCore;
using TruequeU.Interfaces;
using TruequeU.Models;
using TruequeU.Persistence;

namespace TruequeU.Services
{
    public class ModerationLogService : IModerationLogService
    {
        private readonly ApplicationDbContext _context;

        public ModerationLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ModerationLog>> GetAll(
            string? role,
            string? action,
            int? resultCode,
            DateTime? from,
            DateTime? to)
        {
            var query = _context.ModerationLogs.AsQueryable();

            if (!string.IsNullOrEmpty(role))
                query = query.Where(l => l.UserRole == role);

            if (!string.IsNullOrEmpty(action))
                query = query.Where(l => l.Action.Contains(action));

            if (resultCode.HasValue)
                query = query.Where(l => l.ResultCode == resultCode);

            if (from.HasValue)
                query = query.Where(l => l.CreatedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(l => l.CreatedAt <= to.Value);

            return await query
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<ModerationLog> GetById(Guid logId)
        {
            var log = await _context.ModerationLogs
                .FirstOrDefaultAsync(l => l.LogId == logId);

            if (log == null)
                throw new KeyNotFoundException("El log no existe.");

            return log;
        }
    }
}