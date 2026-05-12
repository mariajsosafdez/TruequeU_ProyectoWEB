namespace TruequeU.Models.DTO
{
    public class MessageResponseDTO
    {
        public Guid MessageId { get; set; }
        public Guid ChatId { get; set; }

        public Guid SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
