namespace TruequeU.Models.DTO
{
    public class ChatSummaryDTO
    {
        public Guid ChatId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Campos del otro participante 
        public Guid OtherParticipantId { get; set; }
        public string OtherParticipantName { get; set; } = string.Empty;
        public float OtherParticipantPuntuacion { get; set; }

        public string? LastMessagePreview { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public int UnreadCount { get; set; }
    }
}
