using System.ComponentModel.DataAnnotations;
using TruequeU.Enums;

namespace TruequeU.Models.DTO
{
    public class ResolveReportDto
    {
        [Required]
        public ReportStatus Status { get; set; }
    }
}
