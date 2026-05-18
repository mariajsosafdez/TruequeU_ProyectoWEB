using System.ComponentModel.DataAnnotations;

namespace TruequeU.Models
{
    public class ModerationLog
    {
        [Key]
        public Guid LogId { get; set; } = Guid.NewGuid();

        public string UserId { get; set; } = string.Empty;      // UserId pq puede ser Admin o Cliente
        public string UserEmail { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;      // Que método usa
        public int ResultCode { get; set; }                    

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}