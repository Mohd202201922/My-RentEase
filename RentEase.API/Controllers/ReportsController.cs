using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.DTOs;
using PropertyLeasing.API.Models;

namespace PropertyLeasing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "PropertyManager")]
public class ReportsController : ControllerBase
{
    private readonly PropertyLeasingDbContext _db;

    public ReportsController(PropertyLeasingDbContext db)
    {
        _db = db;
    }

    // GET api/reports/occupancy
    [HttpGet("occupancy")]
    public async Task<IActionResult> GetOccupancyReport()
    {
        var report = await _db.Properties
            .Include(p => p.Units)
            .Select(p => new OccupancyReportDto
            {
                PropertyName   = p.Name,
                TotalUnits     = p.Units.Count,
                OccupiedUnits  = p.Units.Count(u => u.AvailabilityStatus == "Occupied"),
                AvailableUnits = p.Units.Count(u => u.AvailabilityStatus == "Available"),
                OccupancyRate  = p.Units.Count == 0 ? 0 :
                    Math.Round((double)p.Units.Count(u => u.AvailabilityStatus == "Occupied")
                    / p.Units.Count * 100, 2)
            })
            .ToListAsync();

        return Ok(report);
    }

    // GET api/reports/maintenance
    [HttpGet("maintenance")]
    public async Task<IActionResult> GetMaintenanceReport()
    {
        var all = await _db.MaintenanceRequests.ToListAsync();
        var statusLookup = await _db.MaintenanceRequestStatuses
            .ToDictionaryAsync(s => s.StatusId, s => s.StatusName);

        var resolved = all.Where(r => r.ResolvedAt.HasValue).ToList();
        double avgHours = resolved.Any()
            ? resolved.Average(r => (r.ResolvedAt!.Value - r.SubmittedAt).TotalHours)
            : 0;

        var report = new MaintenanceReportDto
        {
            TotalRequests      = all.Count,
            PendingRequests    = all.Count(r => MatchesStatus(r.StatusId, statusLookup, "Submitted", "Assigned")),
            InProgressRequests = all.Count(r => MatchesStatus(r.StatusId, statusLookup, "InProgress")),
            ResolvedRequests   = all.Count(r => MatchesStatus(r.StatusId, statusLookup, "Resolved", "Closed")),
            AvgResolutionHours = Math.Round(avgHours, 2)
        };

        return Ok(report);
    }

    // GET api/reports/payments
    [HttpGet("payments")]
    public async Task<IActionResult> GetPaymentReport()
    {
        var payments = await _db.PaymentRecords.ToListAsync();

        var report = new PaymentReportDto
        {
            TotalDue     = payments.Sum(p => p.AmountDue),
            TotalPaid    = payments.Sum(p => p.AmountPaid ?? 0),
            TotalOverdue = payments
                .Where(p => p.PaymentStatus == "Overdue" || 
                           (p.PaymentStatus == "Pending" && p.DueDate < DateTime.Now))
                .Sum(p => p.AmountDue - (p.AmountPaid ?? 0)),
            OverdueCount = payments
                .Count(p => p.PaymentStatus == "Overdue" || 
                           (p.PaymentStatus == "Pending" && p.DueDate < DateTime.Now))
        };

        return Ok(report);
    }

    // GET api/reports/applications
    [HttpGet("applications")]
    public async Task<IActionResult> GetApplicationsReport()
    {
        var apps = await _db.LeaseApplications
            .Include(a => a.Unit).ThenInclude(u => u.Property)
            .Include(a => a.User)
            .Include(a => a.StatusHistory)
                .ThenInclude(h => h.Status)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new LeaseApplicationDto
            {
                ApplicationId       = a.ApplicationId,
                TenantName          = a.User.FullName,
                UnitNumber          = a.Unit.UnitNumber,
                PropertyName        = a.Unit.Property.Name,
                RequestedStartDate  = a.RequestedStartDate,
                RequestedEndDate    = a.RequestedEndDate,
                Status              = a.StatusHistory
                    .Where(h => h.IsCurrent)
                    .Select(h => h.Status.StatusName)
                    .FirstOrDefault() ?? "Unknown",
                Notes               = a.Notes,
                CreatedAt           = a.CreatedAt
            })
            .ToListAsync();

        return Ok(apps);
    }

    private static bool MatchesStatus(int? statusId, IReadOnlyDictionary<int, string> statusLookup, params string[] names)
    {
        if (!statusId.HasValue || !statusLookup.TryGetValue(statusId.Value, out var statusName))
        {
            return false;
        }

        return names.Contains(statusName, StringComparer.OrdinalIgnoreCase);
    }
}
