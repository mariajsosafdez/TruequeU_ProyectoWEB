using System.ComponentModel.DataAnnotations;
using TruequeU.Models;

public class CreateReportDto : IValidatableObject
{
    public Guid? ReportedUserId { get; set; }
    public Guid? ReportedListingId { get; set; }

    [Required]
    public ReportReason Reason { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
    {
        // Ninguno de los dos presentes
        if (ReportedUserId == null && ReportedListingId == null)
            yield return new ValidationResult(
                "Debe reportar un usuario o un listing.",
                [nameof(ReportedUserId), nameof(ReportedListingId)]
            );

        // Los dos presentes al mismo tiempo
        if (ReportedUserId != null && ReportedListingId != null)
            yield return new ValidationResult(
                "No puedes reportar un usuario y un listing al mismo tiempo.",
                [nameof(ReportedUserId), nameof(ReportedListingId)]
            );
    }
}