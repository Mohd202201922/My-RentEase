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

    private async Task<User?> GetAppUserAsync()
    {
        var identity = await _userManager.GetUserAsync(User);
        if (identity == null) return null;
        return await _db.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identity.Id);
    }

    public async Task<IActionResult> Index(string? status)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var query = _db.LeaseApplications
            .Include(a => a.Unit).ThenInclude(u => u.Property)
            .Include(a => a.User)
            .AsQueryable();

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
                CreatedAt = a.CreatedAt,
                IsPaymentApproved = a.IsPaymentApproved,
                PaymentDate = a.PaymentDate
            })
            .ToListAsync();

        await AutoRejectExpiredScreenings();
        ViewBag.Status = status;
        return View(apps);
    }

    private async Task AutoRejectExpiredScreenings()
    {
        var expiredScreenings = await _db.ScreeningAppointments
            .Include(s => s.Application)
            .Where(s => s.ScheduledDate < DateTime.Now && s.Status != "Completed" && s.Application.Status == "Screening")
            .ToListAsync();

        foreach (var screening in expiredScreenings)
        {
            screening.Application.Status = "Rejected";
            screening.Application.UpdatedAt = DateTime.Now;
            await _notifier.SendAsync(screening.Application.UserId,
                "Your lease application has been rejected because you missed the scheduled screening appointment.",
                "LeaseUpdate");
        }
        await _db.SaveChangesAsync();
    }

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

        var minStartDate = DateTime.Now.AddDays(3).Date;
        return View(new CreateLeaseApplicationViewModel
        {
            UnitId = unit.UnitId,
            UnitNumber = unit.UnitNumber,
            PropertyName = unit.Property.Name,
            MonthlyRent = unit.MonthlyRent,
            RequestedStartDate = minStartDate,
            RequestedEndDate = minStartDate.AddYears(1),
            MinStartDate = minStartDate
        });
    }

    [Authorize(Roles = "Tenant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(CreateLeaseApplicationViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var existing = await _db.LeaseApplications
            .AnyAsync(a => a.UnitId == model.UnitId && a.UserId == appUser.UserId
                        && (a.Status == "Screening" || a.Status == "Approved"));
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
            Status = "Screening",
            CreatedAt = DateTime.Now
        };
        _db.LeaseApplications.Add(application);
        await _db.SaveChangesAsync();

        await _notifier.SendAsync(appUser.UserId,
            "Your lease application has been submitted. Please book a screening appointment to proceed.",
            "LeaseUpdate");

        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
        {
            await _notifier.SendAsync(mgr.UserId,
                $"New lease application from {appUser.FullName} for unit {model.UnitNumber}. Status: Screening.",
                "LeaseUpdate");
        }

        TempData["Success"] = "Application submitted! Please book a screening appointment.";
        return RedirectToAction("Book", "Screening", new { applicationId = application.ApplicationId });
    }

    public async Task<IActionResult> Details(int id)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var application = await _db.LeaseApplications
            .Include(a => a.Unit).ThenInclude(u => u.Property)
            .Include(a => a.User)
            .Include(a => a.ScreeningAppointments)
            .FirstOrDefaultAsync(a => a.ApplicationId == id);

        if (application == null) return NotFound();
        if (appUser.Role == "Tenant" && application.UserId != appUser.UserId)
            return Unauthorized();

        var screening = application.ScreeningAppointments.OrderByDescending(s => s.CreatedAt).FirstOrDefault();

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
            ScreeningDate = screening?.ScheduledDate,
            ScreeningId = screening?.ScreeningId,
            IsPaymentApproved = application.IsPaymentApproved,
            PaymentDate = application.PaymentDate
        };
        return View(model);
    }

    [Authorize(Roles = "PropertyManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApprovePayment(int applicationId)
    {
        var application = await _db.LeaseApplications
            .Include(a => a.User)
            .Include(a => a.Unit)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        if (application == null) return NotFound();

        if (application.Status != "Screening")
        {
            TempData["Error"] = "Only screening applications can be approved for payment.";
            return RedirectToAction("Index");
        }

        var screening = await _db.ScreeningAppointments
            .FirstOrDefaultAsync(s => s.ApplicationId == applicationId && s.Status == "Completed");

        if (screening == null)
        {
            TempData["Error"] = "Screening must be completed before payment approval.";
            return RedirectToAction("Index");
        }

        application.IsPaymentApproved = true;
        application.PaymentApprovedAt = DateTime.Now;
        application.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        await _notifier.SendAsync(application.UserId,
            $"Great news! Your lease application for Unit {application.Unit.UnitNumber} has been approved for payment. Please proceed to make the payment.",
            "LeaseUpdate");

        TempData["Success"] = "Payment approval granted. Tenant can now pay.";
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> Pay(int id)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var application = await _db.LeaseApplications
            .Include(a => a.Unit)
            .FirstOrDefaultAsync(a => a.ApplicationId == id && a.UserId == appUser.UserId);

        if (application == null) return NotFound();

        if (!application.IsPaymentApproved)
        {
            TempData["Error"] = "Payment has not been approved yet.";
            return RedirectToAction("Details", new { id });
        }
        if (application.PaymentDate != null)
        {
            TempData["Error"] = "Payment already processed.";
            return RedirectToAction("Details", new { id });
        }

        var model = new PaymentViewModel
        {
            ApplicationId = application.ApplicationId,
            UnitNumber = application.Unit.UnitNumber,
            PropertyName = application.Unit.Property.Name,
            Amount = (application.Unit.MonthlyRent ?? 0) * 2
        };
        return View(model);
    }

    [Authorize(Roles = "Tenant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
    {
        if (!ModelState.IsValid) return View("Pay", model);

        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var application = await _db.LeaseApplications
            .Include(a => a.Unit)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.ApplicationId == model.ApplicationId && a.UserId == appUser.UserId);

        if (application == null) return NotFound();
        if (!application.IsPaymentApproved || application.PaymentDate != null)
        {
            TempData["Error"] = "Payment not allowed.";
            return RedirectToAction("Details", new { id = application.ApplicationId });
        }

        application.PaymentDate = DateTime.Now;
        application.PaymentAmount = model.Amount;
        application.PaymentTransactionId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        application.Status = "Approved";
        application.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        var activeStatus = await _db.LeaseStatuses.FirstOrDefaultAsync(s => s.StatusName == "Active");
        if (activeStatus == null)
        {
            activeStatus = new LeaseStatus { StatusName = "Active", IsActive = true, IsTerminal = false, CreatedAt = DateTime.Now };
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

        _db.LeaseStatusHistories.Add(new LeaseStatusHistory
        {
            LeaseId = lease.LeaseId,
            StatusId = activeStatus.StatusId,
            ChangedAt = DateTime.Now,
            EffectiveDate = DateTime.Now,
            IsCurrent = true
        });

        application.Unit.AvailabilityStatus = "Occupied";
        await _db.SaveChangesAsync();

        await _notifier.SendAsync(application.UserId,
            $"Payment received! Your lease for Unit {application.Unit.UnitNumber} is now active. Welcome!",
            "LeaseUpdate");

        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
        {
            await _notifier.SendAsync(mgr.UserId,
                $"Payment received from {application.User.FullName} for Unit {application.Unit.UnitNumber}. Lease is now active.",
                "LeaseUpdate");
        }

        TempData["Success"] = "Payment successful! Your lease is now active.";
        return RedirectToAction("Details", new { id = application.ApplicationId });
    }

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

        if (newStatus != "Renewal" && newStatus != "Terminated" && newStatus != "Rejected")
        {
            TempData["Error"] = "Invalid status update.";
            return RedirectToAction("Index");
        }

        application.Status = newStatus;
        application.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        await _notifier.SendAsync(application.UserId,
            $"Your lease application for unit {application.Unit.UnitNumber} has been {newStatus.ToLower()}.",
            "LeaseUpdate");

        TempData["Success"] = $"Application status updated to {newStatus}.";
        return RedirectToAction("Index");
    }
}