using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentEase.API.Data;
using RentEase.API.DTOs;

namespace RentEase.API.Controllers;

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
                PropertyName = p.Name,
                TotalUnits = p.Units.Count,
                OccupiedUnits = p.Units.Count(u => u.AvailabilityStatus == "Occupied"),
                AvailableUnits = p.Units.Count(u => u.AvailabilityStatus == "Available"),
                OccupancyRate = p.Units.Count == 0 ? 0 :
                    Math.Round((double)p.Units.Count(u => u.AvailabilityStatus == "Occupied") / p.Units.Count * 100, 2)
            })
            .ToListAsync();

        return Ok(report);
    }

    // GET api/reports/maintenance (unchanged)
    [HttpGet("maintenance")]
    public async Task<IActionResult> GetMaintenanceReport()
    {
        var all = await _db.MaintenanceRequests.Include(r => r.Status).ToListAsync();
        var resolved = all.Where(r => r.ResolvedAt.HasValue).ToList();
        double avgHours = resolved.Any() ? resolved.Average(r => (r.ResolvedAt!.Value - r.SubmittedAt).TotalHours) : 0;
        var report = new MaintenanceReportDto
        {
            TotalRequests = all.Count,
            PendingRequests = all.Count(r => r.Status?.StatusName == "Submitted" || r.Status?.StatusName == "Assigned"),
            InProgressRequests = all.Count(r => r.Status?.StatusName == "InProgress"),
            ResolvedRequests = all.Count(r => r.Status?.StatusName == "Resolved" || r.Status?.StatusName == "Closed"),
            AvgResolutionHours = Math.Round(avgHours, 2)
        };
        return Ok(report);
    }

    // GET api/reports/payments (unchanged)
    [HttpGet("payments")]
    public async Task<IActionResult> GetPaymentReport()
    {
        var payments = await _db.PaymentRecords.ToListAsync();
        var report = new PaymentReportDto
        {
            TotalDue = payments.Sum(p => p.AmountDue),
            TotalPaid = payments.Sum(p => p.AmountPaid ?? 0),
            TotalOverdue = payments.Where(p => p.PaymentStatus == "Overdue" || (p.PaymentStatus == "Pending" && p.DueDate < DateTime.Now))
                                   .Sum(p => p.AmountDue - (p.AmountPaid ?? 0)),
            OverdueCount = payments.Count(p => p.PaymentStatus == "Overdue" || (p.PaymentStatus == "Pending" && p.DueDate < DateTime.Now))
        };
        return Ok(report);
    }

    // GET api/reports/applications (unchanged)
    [HttpGet("applications")]
    public async Task<IActionResult> GetApplicationsReport()
    {
        var apps = await _db.LeaseApplications
            .Include(a => a.Unit).ThenInclude(u => u.Property).ThenInclude(p => p.Location)
            .Include(a => a.User)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new LeaseApplicationDto
            {
                ApplicationId = a.ApplicationId,
                TenantName = a.User.FullName,
                UnitNumber = a.Unit.UnitNumber,
                PropertyName = a.Unit.Property.Name,
                RequestedStartDate = a.RequestedStartDate,
                RequestedEndDate = a.RequestedEndDate,
                Status = a.Status,
                Notes = a.Notes,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        return Ok(apps);
    }
}