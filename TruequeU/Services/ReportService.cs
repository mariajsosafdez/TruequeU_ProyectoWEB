using Microsoft.EntityFrameworkCore;
using TruequeU.Interfaces;
using TruequeU.Models;
using TruequeU.Models.DTO;
using TruequeU.Persistence;

namespace TruequeU.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }
        private ReportResponseDto MapToResponseDto(Report report)
        {
            return new ReportResponseDto
            {
                ReportId = report.ReportId,
                ReportedBy = report.ReportedBy,
                ReporterName = report.Reporter?.NombreCliente ?? string.Empty,
                ReportedUserId = report.ReportedUserId,
                ReportedUserName = report.ReportedUser?.NombreCliente,
                ReportedListingId = report.ReportedListingId,
                ReportedListingTitulo = report.ReportedListing?.Titulo,
                Reason = report.Reason.ToString(),
                Comment = report.Comment,
                Status = report.Status.ToString(),
                CreatedAt = report.CreatedAt
            };
        }

        public async Task<ReportResponseDto> CreateReport(Guid reportedBy, CreateReportDto dto)
        {
            //No valida si llegan ambos nulos o con contenido, ya que eso lo hace el mismo .NET al crear el obj

            if (dto.ReportedUserId == reportedBy)
                throw new InvalidOperationException("No puedes reportarte a ti mismo.");

            // Valida que el usuario reportado exista 
            if (dto.ReportedUserId != null)
            {
                var userExists = await _context.Clients
                    .AnyAsync(c => c.ClientId == dto.ReportedUserId);
                if (!userExists)
                    throw new KeyNotFoundException("El usuario reportado no existe.");
            }

            // Valida que el listing reportado exista
            if (dto.ReportedListingId != null)
            {
                var listingExists = await _context.Listings
                    .AnyAsync(l => l.IdListing == dto.ReportedListingId);
                if (!listingExists)
                    throw new KeyNotFoundException("El listing reportado no existe.");
            }

            var newReport = new Report
            {
                ReportId = Guid.NewGuid(),
                ReportedBy = reportedBy,
                ReportedUserId = dto.ReportedUserId,
                ReportedListingId = dto.ReportedListingId,
                Reason = dto.Reason,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow,
                Status = Enums.ReportStatus.Pendiente
            };

            _context.Reports.Add(newReport);
            await _context.SaveChangesAsync();

            var created = await _context.Reports
            .Include(r => r.Reporter)
            .Include(r => r.ReportedUser)
            .Include(r => r.ReportedListing)
            .FirstAsync(r => r.ReportId == newReport.ReportId);

            return MapToResponseDto(created);
        }

        public async Task<List<ReportResponseDto>> GetAllReports()
        {
            var reports = await _context.Reports
            .Include(r => r.Reporter)
            .Include(r => r.ReportedUser)
            .Include(r => r.ReportedListing)
            .Where(r => r.IsActive)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

            return reports.Select(r => MapToResponseDto(r)).ToList();
        }

        public async Task<ReportResponseDto> GetReportById(Guid reportId)
        {
            var report = await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedUser)
                .Include(r => r.ReportedListing)
                .FirstOrDefaultAsync(r => r.ReportId == reportId && r.IsActive);

            if (report == null)
                throw new KeyNotFoundException("El reporte no existe.");

            return MapToResponseDto(report);
        }

        public async Task<List<ReportResponseDto>> GetMyReports(Guid clientId)
        {
            var reports = await _context.Reports
            .Include(r => r.ReportedUser)
            .Include(r => r.ReportedListing)
            .Where(r => r.ReportedBy == clientId && r.IsActive)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

            return reports.Select(r => MapToResponseDto(r)).ToList();
        }

        public async Task<ReportResponseDto> ResolveReport(Guid reportId, ResolveReportDto dto)
        {
            var report = await _context.Reports
         .Include(r => r.Reporter)
         .Include(r => r.ReportedUser)
         .Include(r => r.ReportedListing)
         .FirstOrDefaultAsync(r => r.ReportId == reportId && r.IsActive);

            if (report is null)
                throw new KeyNotFoundException("El reporte no existe.");

            //Solo cambiar el estado a Resuelto o Descartado
            if (dto.Status == Enums.ReportStatus.Pendiente)
                throw new InvalidOperationException("No puedes cambiar el estado a Pendiente.");

            report.Status = dto.Status;
            await _context.SaveChangesAsync();

            return MapToResponseDto(report);
        }

        public async Task DeleteReport(Guid reportId)
        {
            var report = await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportId == reportId);

            if (report == null)
                throw new KeyNotFoundException("El reporte no existe.");

            report.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}