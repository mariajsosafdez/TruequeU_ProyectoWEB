using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruequeU.Models
{
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ChatId { get; set; }
        [Required]
        public Guid BuyerId { get; set; }
        [ForeignKey("BuyerID")]
        public Client? Buyer { get; set; } = null!;

        [Required]
        public Guid SellerId { get; set; }
        [ForeignKey("SellerId")]
        public Client Seller { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
