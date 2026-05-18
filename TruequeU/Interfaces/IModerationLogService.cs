using TruequeU.Models;

public interface IModerationLogService
{
    Task<List<ModerationLog>> GetAll(string? role, string? action, int? resultCode, DateTime? from, DateTime? to);
    Task<ModerationLog> GetById(Guid logId);
}