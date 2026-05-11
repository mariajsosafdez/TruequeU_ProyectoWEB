using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruequeU.Models
{
    public class Message
    {
        [Key]
        public Guid MessageId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ChatId { get; set; }
        [ForeignKey("ChatId")]
        public Chat? Chat { get; set; } = null!;

        [Required]
        public Guid SenderId { get; set; }
        [ForeignKey("SenderId")]
        public Clients? Sender { get; set; } = null!;

        [MaxLength(1000)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
}
