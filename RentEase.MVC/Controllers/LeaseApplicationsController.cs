using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentEase.API.Data;
using RentEase.API.Models;
using RentEase.MVC.Services;
using RentEase.MVC.ViewModels;

namespace RentEase.MVC.Controllers;

[Authorize]
public class LeaseApplicationsController : Controller
{
    private readonly PropertyLeasingDbContext _db;
    private readonly UserManager<AppUser> _userManager;
    private readonly NotificationService _notifier;

    public LeaseApplicationsController(
        PropertyLeasingDbContext db,
        UserManager<AppUser> userManager,
        NotificationService notifier)
    {
        _db = db;
        _userManager = userManager;
        _notifier = notifier;
    }

    // Helper: get the app User from the logged-in identity user
    private async Task<User?> GetAppUserAsync()
    {
        var identity = await _userManager.GetUserAsync(User);
        if (identity == null) return null;
        return await _db.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identity.Id);
    }

    // GET /LeaseApplications — tenant sees their own, manager sees all
    public async Task<IActionResult> Index(string? status)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var query = _db.LeaseApplications
            .Include(a => a.Unit).ThenInclude(u => u.Property)
            .Include(a => a.User)
            .AsQueryable();

        // Tenants only see their own applications
        if (appUser.Role == "Tenant")
            query = query.Where(a => a.UserId == appUser.UserId);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(a => a.Status == status);

        var apps = await query
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new LeaseApplicationListViewModel
            {
                ApplicationId = a.ApplicationId,
                UnitNumber = a.Unit.UnitNumber,
                PropertyName = a.Unit.Property.Name,
                TenantName = a.User.FullName,
                RequestedStartDate = a.RequestedStartDate,
                RequestedEndDate = a.RequestedEndDate,
                Status = a.Status,
                Notes = a.Notes,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        ViewBag.Status = status;
        return View(apps);
    }

    // GET /LeaseApplications/Apply/{unitId}
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> Apply(int unitId)
    {
        var unit = await _db.Units
            .Include(u => u.Property)
            .FirstOrDefaultAsync(u => u.UnitId == unitId);

        if (unit == null) return NotFound();

        if (unit.AvailabilityStatus != "Available")
        {
            TempData["Error"] = "This unit is not available for leasing.";
            return RedirectToAction("UnitDetails", "Properties", new { id = unitId });
        }

        return View(new CreateLeaseApplicationViewModel
        {
            UnitId = unit.UnitId,
            UnitNumber = unit.UnitNumber,
            PropertyName = unit.Property.Name,
            MonthlyRent = unit.MonthlyRent,
            RequestedStartDate = DateTime.Now.AddDays(7),
            RequestedEndDate = DateTime.Now.AddYears(1).AddDays(7)
        });
    }

    // POST /LeaseApplications/Apply
    [Authorize(Roles = "Tenant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(CreateLeaseApplicationViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        // Check no pending/approved application already exists for this unit
        var existing = await _db.LeaseApplications
            .AnyAsync(a => a.UnitId == model.UnitId && a.UserId == appUser.UserId
                        && (a.Status == "Pending" || a.Status == "Screening" || a.Status == "Approved"));
        if (existing)
        {
            TempData["Error"] = "You already have an active application for this unit.";
            return RedirectToAction("Index");
        }

        var application = new LeaseApplication
        {
            UserId = appUser.UserId,
            UnitId = model.UnitId,
            RequestedStartDate = model.RequestedStartDate,
            RequestedEndDate = model.RequestedEndDate,
            Notes = model.Notes,
            Status = "Pending",
            CreatedAt = DateTime.Now
        };

        _db.LeaseApplications.Add(application);
        await _db.SaveChangesAsync();

        // Notify tenant
        await _notifier.SendAsync(appUser.UserId,
            "Your lease application has been submitted. Please book a screening appointment to proceed.",
            "LeaseUpdate");

        // Notify all managers
        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
        {
            await _notifier.SendAsync(mgr.UserId,
                $"New lease application from {appUser.FullName} for unit {model.UnitNumber}. Please review and schedule screening.",
                "LeaseUpdate");
        }

        // Redirect to book screening
        TempData["Success"] = "Application submitted! Please book a screening appointment to continue.";
        return RedirectToAction("Book", "Screening", new { applicationId = application.ApplicationId });
    }

    // GET /LeaseApplications/Details/{id}
    public async Task<IActionResult> Details(int id)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var application = await _db.LeaseApplications
            .Include(a => a.Unit)
            .ThenInclude(u => u.Property)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.ApplicationId == id);

        if (application == null) return NotFound();

        // Check authorization
        if (appUser.Role == "Tenant" && application.UserId != appUser.UserId)
            return Unauthorized();

        var screening = await _db.ScreeningAppointments
            .FirstOrDefaultAsync(s => s.ApplicationId == id);

        var model = new LeaseApplicationDetailViewModel
        {
            ApplicationId = application.ApplicationId,
            UnitNumber = application.Unit.UnitNumber,
            PropertyName = application.Unit.Property.Name,
            PropertyAddress = application.Unit.Property.Address,
            TenantName = application.User.FullName,
            TenantPhone = application.User.Phone ?? "N/A",
            TenantEmail = application.User.Email,
            RequestedStartDate = application.RequestedStartDate,
            RequestedEndDate = application.RequestedEndDate,
            MonthlyRent = application.Unit.MonthlyRent ?? 0,
            Notes = application.Notes,
            Status = application.Status,
            CreatedAt = application.CreatedAt,
            HasScreening = screening != null,
            ScreeningStatus = screening?.Status,
            ScreeningDate = screening?.ScheduledDate
        };

        return View(model);
    }

    // POST /LeaseApplications/UpdateStatus — Manager only
    [Authorize(Roles = "PropertyManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int applicationId, string newStatus)
    {
        var application = await _db.LeaseApplications
            .Include(a => a.Unit)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        if (application == null) return NotFound();

        var oldStatus = application.Status;
        application.Status = newStatus;
        await _db.SaveChangesAsync();

        // If approved directly (without screening flow) - for existing applications
        if (newStatus == "Approved" && oldStatus != "Approved")
        {
            // Get or create the "Active" lease status
            var activeStatus = await _db.LeaseStatuses
                .FirstOrDefaultAsync(s => s.StatusName == "Active");
            if (activeStatus == null)
            {
                activeStatus = new LeaseStatus
                {
                    StatusName = "Active",
                    IsActive = true,
                    IsTerminal = false,
                    CreatedAt = DateTime.Now
                };
                _db.LeaseStatuses.Add(activeStatus);
                await _db.SaveChangesAsync();
            }

            var lease = new Lease
            {
                ApplicationId = application.ApplicationId,
                LeaseStartDate = application.RequestedStartDate ?? DateTime.Now,
                LeaseEndDate = application.RequestedEndDate ?? DateTime.Now.AddYears(1),
                MonthlyRent = application.Unit.MonthlyRent ?? 0,
                SecurityDeposit = (application.Unit.MonthlyRent ?? 0) * 2,
                CreatedAt = DateTime.Now
            };
            _db.Leases.Add(lease);
            await _db.SaveChangesAsync();

            // Create lease status history to set it to Active
            _db.LeaseStatusHistories.Add(new LeaseStatusHistory
            {
                LeaseId = lease.LeaseId,
                StatusId = activeStatus.StatusId,
                ChangedAt = DateTime.Now,
                EffectiveDate = DateTime.Now,
                IsCurrent = true
            });

            application.Unit.AvailabilityStatus = "Occupied";

            // Generate first payment record
            _db.PaymentRecords.Add(new PaymentRecord
            {
                LeaseId = lease.LeaseId,
                AmountDue = lease.MonthlyRent,
                DueDate = lease.LeaseStartDate,
                PaymentStatus = "Pending"
            });
            await _db.SaveChangesAsync();
        }

        // Notify tenant
        await _notifier.SendAsync(application.UserId,
            $"Your lease application for unit {application.Unit.UnitNumber} has been {newStatus.ToLower()}.",
            "LeaseUpdate");

        TempData["Success"] = $"Application status updated to {newStatus}.";
        return RedirectToAction("Index");
    }

    // POST /LeaseApplications/Delete/{id} — Manager only
    [Authorize(Roles = "PropertyManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var application = await _db.LeaseApplications
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.ApplicationId == id);

        if (application == null) return NotFound();

        // Notify tenant
        await _notifier.SendAsync(application.UserId,
            $"Your lease application for has been removed by the manager.",
            "LeaseUpdate");

        _db.LeaseApplications.Remove(application);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Application deleted successfully.";
        return RedirectToAction("Index");
    }

    // GET /LeaseApplications/Resubmit/{id} — Tenant can resubmit rejected application
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> Resubmit(int id)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var application = await _db.LeaseApplications
            .Include(a => a.Unit)
            .ThenInclude(u => u.Property)
            .FirstOrDefaultAsync(a => a.ApplicationId == id && a.UserId == appUser.UserId);

        if (application == null) return NotFound();

        if (application.Status != "Rejected")
        {
            TempData["Error"] = "Only rejected applications can be resubmitted.";
            return RedirectToAction("Index");
        }

        application.Status = "Pending";
        application.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        // Notify managers
        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
        {
            await _notifier.SendAsync(mgr.UserId,
                $"Application for Unit {application.Unit.UnitNumber} has been resubmitted by {appUser.FullName}.",
                "LeaseUpdate");
        }

        TempData["Success"] = "Application resubmitted successfully.";
        return RedirectToAction("Index");
    }
}

// Additional ViewModel for Details page
public class LeaseApplicationDetailViewModel
{
    public int ApplicationId { get; set; }
    public string UnitNumber { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public string PropertyAddress { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string TenantPhone { get; set; } = string.Empty;
    public string TenantEmail { get; set; } = string.Empty;
    public DateTime? RequestedStartDate { get; set; }
    public DateTime? RequestedEndDate { get; set; }
    public decimal MonthlyRent { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool HasScreening { get; set; }
    public string? ScreeningStatus { get; set; }
    public DateTime? ScreeningDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
}