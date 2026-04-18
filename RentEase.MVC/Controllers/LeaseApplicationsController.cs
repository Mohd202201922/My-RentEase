using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using RentEase.API.Models;
using PropertyLeasing.MVC.Services;
using PropertyLeasing.MVC.ViewModels;

namespace PropertyLeasing.MVC.Controllers;

[Authorize]
public class LeaseApplicationsController : Controller
{
    private readonly PropertyLeasingDbContext _db;
    private readonly UserManager<AppUser>     _userManager;
    private readonly NotificationService      _notifier;

    public LeaseApplicationsController(
        PropertyLeasingDbContext db,
        UserManager<AppUser> userManager,
        NotificationService notifier)
    {
        _db          = db;
        _userManager = userManager;
        _notifier    = notifier;
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
            .Include(a => a.StatusHistory)
                .ThenInclude(h => h.Status)
            .AsQueryable();

        // Tenants only see their own applications
        if (appUser.Role == "Tenant")
            query = query.Where(a => a.UserId == appUser.UserId);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(a => a.StatusHistory.Any(h => h.IsCurrent && h.Status.StatusName == status));

        var apps = await query
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new LeaseApplicationListViewModel
            {
                ApplicationId      = a.ApplicationId,
                UnitNumber         = a.Unit.UnitNumber,
                PropertyName       = a.Unit.Property.Name,
                TenantName         = a.User.FullName,
                RequestedStartDate = a.RequestedStartDate,
                RequestedEndDate   = a.RequestedEndDate,
                Status             = a.StatusHistory
                    .Where(h => h.IsCurrent)
                    .Select(h => h.Status.StatusName)
                    .FirstOrDefault() ?? "Unknown",
                Notes              = a.Notes,
                CreatedAt          = a.CreatedAt
            })
            .ToListAsync();

        ViewBag.Status = status;
        return View(apps);
    }

    // GET /LeaseApplications/Apply/{unitId}
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> Apply(int unitId)
    {
        var unit = await _db.Units.Include(u => u.Property).FirstOrDefaultAsync(u => u.UnitId == unitId);
        if (unit == null) return NotFound();

        if (unit.AvailabilityStatus != "Available")
        {
            TempData["Error"] = "This unit is not available for leasing.";
            return RedirectToAction("UnitDetails", "Properties", new { id = unitId });
        }

        return View(new CreateLeaseApplicationViewModel
        {
            UnitId       = unit.UnitId,
            UnitNumber   = unit.UnitNumber,
            PropertyName = unit.Property.Name,
            MonthlyRent  = unit.MonthlyRent
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
            .Include(a => a.StatusHistory)
                .ThenInclude(h => h.Status)
            .AnyAsync(a => a.UnitId == model.UnitId
                && a.UserId == appUser.UserId
                && a.StatusHistory.Any(h => h.IsCurrent
                    && (h.Status.StatusName == "Pending"
                        || h.Status.StatusName == "Screening"
                        || h.Status.StatusName == "Approved")));
        if (existing)
        {
            TempData["Error"] = "You already have an active application for this unit.";
            return RedirectToAction("Index");
        }

        var application = new LeaseApplication
        {
            UserId             = appUser.UserId,
            UnitId             = model.UnitId,
            RequestedStartDate = model.RequestedStartDate,
            RequestedEndDate   = model.RequestedEndDate,
            Notes              = model.Notes,
            CreatedAt          = DateTime.Now
        };

        _db.LeaseApplications.Add(application);
        await _db.SaveChangesAsync();

        var pendingStatusId = await _db.LeaseApplicationStatuses
            .Where(s => s.StatusName == "Pending")
            .Select(s => (int?)s.StatusId)
            .FirstOrDefaultAsync();
        if (!pendingStatusId.HasValue)
        {
            TempData["Error"] = "Lease application statuses are missing from the database.";
            return RedirectToAction("Index");
        }

        _db.LeaseApplicationStatusHistories.Add(new LeaseApplicationStatusHistory
        {
            ApplicationId = application.ApplicationId,
            StatusId = pendingStatusId.Value,
            ChangedAt = DateTime.Now,
            IsCurrent = true
        });
        await _db.SaveChangesAsync();

        // Notify tenant
        await _notifier.SendAsync(appUser.UserId,
            "Your lease application has been submitted and is under review.",
            "LeaseUpdate");

        // Notify all managers
        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
            await _notifier.SendAsync(mgr.UserId,
                $"New lease application from {appUser.FullName} for unit {model.UnitNumber}.",
                "LeaseUpdate");

        TempData["Success"] = $"Application submitted! Your ticket is being reviewed.";
        return RedirectToAction("Index");
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
            .Include(a => a.StatusHistory)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        if (application == null) return NotFound();
        var appUser = await GetAppUserAsync();
        var statusRow = await _db.LeaseApplicationStatuses.FirstOrDefaultAsync(s => s.StatusName == newStatus);
        if (statusRow == null)
        {
            TempData["Error"] = $"Unknown lease application status: {newStatus}.";
            return RedirectToAction("Index");
        }

        foreach (var history in application.StatusHistory.Where(h => h.IsCurrent))
        {
            history.IsCurrent = false;
        }

        _db.LeaseApplicationStatusHistories.Add(new LeaseApplicationStatusHistory
        {
            ApplicationId = application.ApplicationId,
            StatusId = statusRow.StatusId,
            ChangedAt = DateTime.Now,
            ChangedByUserId = appUser?.UserId,
            IsCurrent = true
        });

        // If approved → create Lease and mark unit as Occupied
        if (newStatus == "Approved")
        {
            var lease = new Lease
            {
                ApplicationId   = application.ApplicationId,
                LeaseStartDate  = application.RequestedStartDate ?? DateTime.Now,
                LeaseEndDate    = application.RequestedEndDate   ?? DateTime.Now.AddYears(1),
                MonthlyRent     = application.Unit.MonthlyRent ?? 0,
                SecurityDeposit = (application.Unit.MonthlyRent ?? 0) * 2,
                CreatedAt       = DateTime.Now
            };
            _db.Leases.Add(lease);
            await _db.SaveChangesAsync();

            var activeLeaseStatusId = await _db.LeaseStatuses
                .Where(s => s.StatusName == "Active")
                .Select(s => (int?)s.StatusId)
                .FirstOrDefaultAsync();
            if (activeLeaseStatusId.HasValue)
            {
                _db.LeaseStatusHistories.Add(new LeaseStatusHistory
                {
                    LeaseId = lease.LeaseId,
                    StatusId = activeLeaseStatusId.Value,
                    ChangedAt = DateTime.Now,
                    EffectiveDate = lease.LeaseStartDate,
                    ChangedByUserId = appUser?.UserId,
                    IsCurrent = true
                });
            }

            application.Unit.AvailabilityStatus = "Occupied";

            // Generate first payment record
            _db.PaymentRecords.Add(new PaymentRecord
            {
                LeaseId       = lease.LeaseId,
                AmountDue     = lease.MonthlyRent,
                DueDate       = lease.LeaseStartDate,
                PaymentStatus = "Pending"
            });
        }

        await _db.SaveChangesAsync();

        // Notify tenant
        await _notifier.SendAsync(application.UserId,
            $"Your lease application for unit {application.Unit.UnitNumber} has been {newStatus.ToLower()}.",
            "LeaseUpdate");

        TempData["Success"] = $"Application status updated to {newStatus}.";
        return RedirectToAction("Index");
    }
}
