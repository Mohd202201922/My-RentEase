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

    private async Task AutoRejectExpiredPendingApplications()
    {
        var cutoff = DateTime.Now.AddHours(-24);
        var expired = await _db.LeaseApplications
            .Include(a => a.Unit)
            .Where(a => a.Status == "Pending" && a.CreatedAt <= cutoff)
            .ToListAsync();

        foreach (var app in expired)
        {
            app.Status = "Rejected";
            app.UpdatedAt = DateTime.Now;
            app.Unit.AvailabilityStatus = "Available";
            await _notifier.SendAsync(app.UserId,
                "Your lease application has expired because you did not complete payment within 24 hours.",
                "LeaseUpdate");
        }
        await _db.SaveChangesAsync();
    }

    private async Task ProcessTerminatedUnits()
    {
        var today = DateTime.Now.Date;
        var toMoveOut = await _db.LeaseApplications
            .Include(a => a.Unit)
            .Where(a => a.Status == "Terminated" && a.TerminationMoveOutDate.HasValue && a.TerminationMoveOutDate.Value.Date <= today)
            .ToListAsync();

        foreach (var app in toMoveOut)
        {
            app.Unit.AvailabilityStatus = "Available";
            await _db.SaveChangesAsync();
            await _notifier.SendAsync(app.UserId,
                "Your lease has ended. You have moved out. Thank you.",
                "LeaseUpdate");
        }
    }

    public async Task<IActionResult> Index(string? status)
    {
        await AutoRejectExpiredPendingApplications();
        await ProcessTerminatedUnits();

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
                PaymentDate = a.PaymentDate,
                TerminationRequested = a.TerminationRequested
            })
            .ToListAsync();

        ViewBag.Status = status;
        return View(apps);
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
                        && (a.Status == "Pending" || a.Status == "Approved"));
        if (existing)
        {
            TempData["Error"] = "You already have an active application for this unit.";
            return RedirectToAction("Index");
        }

        var unit = await _db.Units.FindAsync(model.UnitId);
        if (unit == null) return NotFound();

        unit.AvailabilityStatus = "Occupied";

        var application = new LeaseApplication
        {
            UserId = appUser.UserId,
            UnitId = model.UnitId,
            RequestedStartDate = model.RequestedStartDate,
            RequestedEndDate = model.RequestedEndDate,
            Notes = model.Notes,
            Status = "Pending",
            CreatedAt = DateTime.Now,
            IsPaymentApproved = false
        };
        _db.LeaseApplications.Add(application);
        await _db.SaveChangesAsync();

        await _notifier.SendAsync(appUser.UserId,
            "Your lease application has been submitted. You have 24 hours to complete payment.",
            "LeaseUpdate");

        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
        {
            await _notifier.SendAsync(mgr.UserId,
                $"New lease application from {appUser.FullName} for unit {model.UnitNumber}. Status: Pending.",
                "LeaseUpdate");
        }

        TempData["Success"] = "Application submitted! You have 24 hours to complete payment. Unit is now reserved for you.";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(int id)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var application = await _db.LeaseApplications
            .Include(a => a.Unit).ThenInclude(u => u.Property)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.ApplicationId == id);

        if (application == null) return NotFound();
        if (appUser.Role == "Tenant" && application.UserId != appUser.UserId)
            return Unauthorized();

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
            IsPaymentApproved = application.IsPaymentApproved,
            PaymentDate = application.PaymentDate,
            TerminationRequested = application.TerminationRequested,
            TerminationRequestDate = application.TerminationRequestDate,
            TerminationApprovedAt = application.TerminationApprovedAt,
            TerminationMoveOutDate = application.TerminationMoveOutDate
        };
        return View(model);
    }

    [Authorize(Roles = "PropertyManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PermitPayment(int applicationId)
    {
        var application = await _db.LeaseApplications
            .Include(a => a.User)
            .Include(a => a.Unit)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        if (application == null) return NotFound();

        if (application.Status != "Pending")
        {
            TempData["Error"] = "Only pending applications can be permitted for payment.";
            return RedirectToAction("Index");
        }

        application.IsPaymentApproved = true;
        application.PaymentApprovedAt = DateTime.Now;
        application.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        await _notifier.SendAsync(application.UserId,
            $"Your lease application for Unit {application.Unit.UnitNumber} has been approved for payment. Please proceed to pay within 24 hours of application submission.",
            "LeaseUpdate");

        TempData["Success"] = "Payment permission granted. Tenant can now pay.";
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "PropertyManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectApplication(int applicationId)
    {
        var application = await _db.LeaseApplications
            .Include(a => a.User)
            .Include(a => a.Unit)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        if (application == null) return NotFound();

        if (application.Status != "Pending")
        {
            TempData["Error"] = "Only pending applications can be rejected.";
            return RedirectToAction("Index");
        }

        application.Status = "Rejected";
        application.UpdatedAt = DateTime.Now;
        application.Unit.AvailabilityStatus = "Available";
        await _db.SaveChangesAsync();

        await _notifier.SendAsync(application.UserId,
            $"Your lease application for Unit {application.Unit.UnitNumber} has been rejected.",
            "LeaseUpdate");

        TempData["Success"] = "Application rejected. Unit is now available.";
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Tenant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestTermination(int applicationId)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var application = await _db.LeaseApplications
            .Include(a => a.Unit)   // ✅ Include Unit to avoid null reference
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId && a.UserId == appUser.UserId);

        if (application == null) return NotFound();

        if (application.Status != "Approved")
        {
            TempData["Error"] = "Only approved leases can be terminated.";
            return RedirectToAction("Details", new { id = applicationId });
        }

        if (application.TerminationRequested)
        {
            TempData["Error"] = "Termination already requested.";
            return RedirectToAction("Details", new { id = applicationId });
        }

        application.TerminationRequested = true;
        application.TerminationRequestDate = DateTime.Now;
        await _db.SaveChangesAsync();

        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
        {
            await _notifier.SendAsync(mgr.UserId,
                $"Tenant {appUser.FullName} has requested termination for Unit {application.Unit.UnitNumber}.",
                "LeaseUpdate");
        }

        TempData["Success"] = "Termination request submitted. Property manager will review.";
        return RedirectToAction("Details", new { id = applicationId });
    }

    [Authorize(Roles = "PropertyManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveTermination(int applicationId)
    {
        var application = await _db.LeaseApplications
            .Include(a => a.Unit)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        if (application == null) return NotFound();

        if (application.Status != "Approved" || !application.TerminationRequested)
        {
            TempData["Error"] = "Invalid termination request.";
            return RedirectToAction("Index");
        }

        application.Status = "Terminated";
        application.TerminationApprovedAt = DateTime.Now;
        application.TerminationMoveOutDate = DateTime.Now.AddDays(3);
        application.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        await _notifier.SendAsync(application.UserId,
            $"Your termination request has been approved. You have 3 days to move out. After that, the unit will be made available.",
            "LeaseUpdate");

        TempData["Success"] = "Termination approved. Tenant has 3 days to vacate.";
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> Pay(int id)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var application = await _db.LeaseApplications
            .Include(a => a.Unit).ThenInclude(u => u.Property)
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
        if (application.Status != "Pending")
        {
            TempData["Error"] = "Only pending applications can be paid.";
            return RedirectToAction("Details", new { id });
        }

        if (application.CreatedAt.AddHours(24) < DateTime.Now)
        {
            TempData["Error"] = "Payment window expired (24 hours). Application has been rejected.";
            return RedirectToAction("Index");
        }

        var model = new PaymentViewModel
        {
            ApplicationId = application.ApplicationId,
            UnitNumber = application.Unit.UnitNumber,
            PropertyName = application.Unit.Property.Name,
            Amount = (application.Unit.MonthlyRent ?? 0) * 2,
            LeaseStartDate = application.RequestedStartDate,
            LeaseEndDate = application.RequestedEndDate,
            MonthlyRent = application.Unit.MonthlyRent ?? 0,
            SecurityDeposit = (application.Unit.MonthlyRent ?? 0) * 2
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
        if (application.Status != "Pending")
        {
            TempData["Error"] = "Only pending applications can be paid.";
            return RedirectToAction("Details", new { id = application.ApplicationId });
        }

        // Simulate payment
        application.PaymentDate = DateTime.Now;
        application.PaymentAmount = model.Amount;
        application.PaymentTransactionId = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        application.Status = "Approved";
        application.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        // Create lease record
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
    public async Task<IActionResult> TerminateApplication(int applicationId)
    {
        var application = await _db.LeaseApplications
            .Include(a => a.Unit)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        if (application == null) return NotFound();

        if (application.Status != "Approved")
        {
            TempData["Error"] = "Only approved applications can be terminated.";
            return RedirectToAction("Index");
        }

        application.Status = "Terminated";
        application.UpdatedAt = DateTime.Now;
        application.TerminationApprovedAt = DateTime.Now;
        application.TerminationMoveOutDate = DateTime.Now.AddDays(3);
        await _db.SaveChangesAsync();

        await _notifier.SendAsync(application.UserId,
            $"Your lease for Unit {application.Unit.UnitNumber} has been terminated. You have 3 days to vacate.",
            "LeaseUpdate");

        TempData["Success"] = "Lease terminated. Unit will become available after 3 days.";
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "PropertyManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RenewApplication(int applicationId)
    {
        var application = await _db.LeaseApplications
            .Include(a => a.Unit)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        if (application == null) return NotFound();

        if (application.Status != "Approved")
        {
            TempData["Error"] = "Only approved applications can be renewed.";
            return RedirectToAction("Index");
        }

        application.Status = "Renewal";
        application.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        await _notifier.SendAsync(application.UserId,
            $"Your lease for Unit {application.Unit.UnitNumber} has been renewed.",
            "LeaseUpdate");

        TempData["Success"] = "Lease renewed.";
        return RedirectToAction("Index");
    }
}