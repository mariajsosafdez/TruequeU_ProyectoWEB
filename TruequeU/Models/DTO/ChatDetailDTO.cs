namespace TruequeU.Models.DTO
{
    public class ChatDetailDTO
    {
        public Guid ChatId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid BuyerId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public float BuyerPuntuacion { get; set; }

        public Guid SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public float SellerPuntuacion { get; set; }
    }
}
