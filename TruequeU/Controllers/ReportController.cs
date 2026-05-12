using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TruequeU.Interfaces;
using TruequeU.Models;
namespace TruequeU.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateReport(
            [FromQuery] Guid? reportedUserId,
            [FromQuery] Guid? reportedListingId,
            [FromQuery] ReportReason reason,
            [FromQuery] string? comment)
        {
            try
            {
                var clientId = GetCurrentClientId();
                var report = await _reportService.CreateReport(
                    clientId, reportedUserId, reportedListingId, reason, comment);
                return CreatedAtAction(nameof(GetReportById), new { reportId = report.ReportId }, report);
            }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _reportService.GetAllReports();
            return Ok(reports);
        }

        [HttpGet("me")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetMyReports()
        {
            var clientId = GetCurrentClientId();
            var reports = await _reportService.GetMyReports(clientId);
            return Ok(reports);
        }

        [HttpGet("{reportId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReportById(Guid reportId)
        {
            try
            {
                var report = await _reportService.GetReportById(reportId);
                return Ok(report);
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
        }

        [HttpPatch("{reportId}/resolve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResolveReport(Guid reportId, [FromQuery] ReportStatus status)
        {
            try
            {
                var report = await _reportService.ResolveReport(reportId, status);
                return Ok(report);
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
        }

        [HttpDelete("{reportId}")] //Aunque diga delete, hace softDelete
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReport(Guid reportId)
        {
            try
            {
                await _reportService.DeleteReport(reportId);
                return NoContent();
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
        }

        private Guid GetCurrentClientId() =>
            Guid.Parse(User.FindFirstValue("ClientId")!);

        public IActionResult Index()
        {
            return View();
        }
    }
}
