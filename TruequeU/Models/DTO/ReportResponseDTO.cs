public class ReportResponseDto
{
    public Guid ReportId { get; set; }

    public Guid ReportedBy { get; set; }
    public string ReporterName { get; set; } = string.Empty;

    public Guid? ReportedUserId { get; set; }
    public string? ReportedUserName { get; set; }

    public Guid? ReportedListingId { get; set; }
    public string? ReportedListingTitulo { get; set; }

    public string Reason { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}