using TruequeU.Models;

namespace TruequeU.Interfaces
{
    public interface IReportService
    {
        Task<Report> CreateReport(Guid reportedBy, Guid? reportedUserId, Guid? reportedListingId, ReportReason reason, string? comment);

        Task<List<Report>> GetAllReports();

        Task<Report> GetReportById(Guid reportId);

        Task<List<Report>> GetMyReports(Guid clientId);

        Task<Report> ResolveReport(Guid reportId, ReportStatus status);

        Task DeleteReport(Guid reportId);
    }
}
