using TruequeU.Models;
using TruequeU.Models.DTO;

namespace TruequeU.Interfaces
{
    public interface IReportService
    {
        Task<ReportResponseDto> CreateReport(Guid reportedBy, CreateReportDto dto);

        Task<List<ReportResponseDto>> GetAllReports();

        Task<ReportResponseDto> GetReportById(Guid reportId);

        Task<List<ReportResponseDto>> GetMyReports(Guid clientId);

        Task<ReportResponseDto> ResolveReport(Guid reportId, ResolveReportDto dto);

        Task DeleteReport(Guid reportId);
    }
}
