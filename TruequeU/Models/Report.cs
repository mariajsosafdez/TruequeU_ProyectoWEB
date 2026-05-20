using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TruequeU.Enums;

namespace TruequeU.Models
{
    public class Report
    {
        [Key]
        public Guid ReportId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ReportedBy { get; set; }
        [ForeignKey("ReportedBy")]
        public Clients? Reporter { get; set; }

        // Reporte contra un usuario
        public Guid? ReportedUserId { get; set; }
        [ForeignKey("ReportedUserId")]
        public Clients? ReportedUser { get; set; }

        // Reporte contra un listing
        public Guid? ReportedListingId { get; set; }
        [ForeignKey("ReportedListingId")]
        public Listings? ReportedListing { get; set; }

        [Required]
        public ReportReason Reason { get; set; }
        [MaxLength(500)]
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ReportStatus Status { get; set; } = ReportStatus.Pendiente;
        public bool IsActive { get; set; } = true;
    }
}