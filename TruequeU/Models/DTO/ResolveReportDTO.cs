using System.ComponentModel.DataAnnotations;

namespace TruequeU.Models.DTO
{
    public class ResolveReportDto
    {
        [Required]
        public ReportStatus Status { get; set; }
    }
}
