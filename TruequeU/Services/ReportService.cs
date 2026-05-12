using Microsoft.EntityFrameworkCore;
using TruequeU.Interfaces;
using TruequeU.Models;
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

        public async Task<Report> CreateReport(Guid reportedBy, Guid? reportedUserId, Guid? reportedListingId, ReportReason reason, string? comment)
        {
            // Al menos uno debe estar presente
            if (reportedUserId is null && reportedListingId is null)
                throw new InvalidOperationException("Debe reportar un usuario o un listing.");

            if (reportedUserId == reportedBy)
                throw new InvalidOperationException("No puedes reportarte a ti mismo.");

            // Valida que el usuario reportado exista 
            if (reportedUserId != null)
            {
                var userExists = await _context.Clients
                    .AnyAsync(c => c.ClientId == reportedUserId);
                if (!userExists)
                    throw new KeyNotFoundException("El usuario reportado no existe.");
            }

            // Valida que el listing reportado exista
            if (reportedListingId != null)
            {
                var listingExists = await _context.Listings
                    .AnyAsync(l => l.IdListing == reportedListingId);
                if (!listingExists)
                    throw new KeyNotFoundException("El listing reportado no existe.");
            }

            var newReport = new Report
            {
                ReportId = Guid.NewGuid(),
                ReportedBy = reportedBy,
                ReportedUserId = reportedUserId,
                ReportedListingId = reportedListingId,
                Reason = reason,
                Comment = comment,
                CreatedAt = DateTime.UtcNow,
                Status = ReportStatus.Pendiente
            };

            _context.Reports.Add(newReport);
            await _context.SaveChangesAsync();

            return newReport;
        }

        public async Task<List<Report>> GetAllReports()
        {
            return await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedUser)
                .Include(r => r.ReportedListing)
                .Where(r => r.IsActive)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Report> GetReportById(Guid reportId)
        {
            var report = await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.ReportedUser)
                .Include(r => r.ReportedListing)
                .FirstOrDefaultAsync(r => r.ReportId == reportId && r.IsActive);

            if (report == null)
                throw new KeyNotFoundException("El reporte no existe.");

            return report;
        }

        public async Task<List<Report>> GetMyReports(Guid clientId)
        {
            return await _context.Reports
                .Include(r => r.ReportedUser)
                .Include(r => r.ReportedListing)
                .Where(r => r.ReportedBy == clientId && r.IsActive)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Report> ResolveReport(Guid reportId, ReportStatus status)
        {
            var report = await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportId == reportId);

            if (report == null)
                throw new KeyNotFoundException("El reporte no existe.");

            // Solo puede cambiar a Resuelto o Descartado
            if (status == ReportStatus.Pendiente)
                throw new InvalidOperationException("No puedes cambiar el estado a Pendiente.");

            report.Status = status;
            await _context.SaveChangesAsync();

            return report;
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