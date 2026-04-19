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
public class ScreeningController : Controller
{
    private readonly PropertyLeasingDbContext _db;
    private readonly UserManager<AppUser> _userManager;
    private readonly NotificationService _notifier;

    public ScreeningController(
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

    // GET: /Screening/Book/{applicationId}
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> Book(int applicationId)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var application = await _db.LeaseApplications
            .Include(a => a.Unit).ThenInclude(u => u.Property)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId && a.UserId == appUser.UserId);

        if (application == null) return NotFound();

        var existingScreening = await _db.ScreeningAppointments
            .FirstOrDefaultAsync(s => s.ApplicationId == applicationId && s.Status != "Cancelled");

        if (existingScreening != null)
        {
            TempData["Error"] = "You already have a screening appointment for this application.";
            return RedirectToAction("Index", "LeaseApplications");
        }

        var model = new BookScreeningViewModel
        {
            ApplicationId = application.ApplicationId,
            UnitId = application.UnitId,
            UnitNumber = application.Unit.UnitNumber,
            PropertyName = application.Unit.Property.Name,
            MonthlyRent = application.Unit.MonthlyRent,
            LeaseStartDate = application.RequestedStartDate ?? DateTime.Now.AddDays(30)  // fallback
        };
        return View(model);
    }

    // POST: /Screening/Book
    [Authorize(Roles = "Tenant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(BookScreeningViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // Reload the application to get the lease start date
        var application = await _db.LeaseApplications.FindAsync(model.ApplicationId);
        if (application == null) return NotFound();

        var leaseStartDate = application.RequestedStartDate ?? DateTime.Now.AddDays(30);

        // Validate that the preferred date is before the lease start date
        if (!model.PreferredDate.HasValue)
        {
            ModelState.AddModelError("PreferredDate", "Please select a preferred date.");
            return View(model);
        }

        if (model.PreferredDate.Value.Date >= leaseStartDate.Date)
        {
            ModelState.AddModelError("PreferredDate", $"Screening must take place before the lease start date ({leaseStartDate:dd MMM yyyy}).");
            return View(model);
        }

        // Validate time: must be between 08:00 and 20:00
        if (string.IsNullOrWhiteSpace(model.PreferredTime))
        {
            ModelState.AddModelError("PreferredTime", "Please select a preferred time.");
            return View(model);
        }

        if (!TimeSpan.TryParse(model.PreferredTime, out var selectedTime))
        {
            ModelState.AddModelError("PreferredTime", "Invalid time format.");
            return View(model);
        }

        var minTime = new TimeSpan(8, 0, 0);
        var maxTime = new TimeSpan(20, 0, 0);
        if (selectedTime < minTime || selectedTime > maxTime)
        {
            ModelState.AddModelError("PreferredTime", "Please select a time between 8:00 AM and 8:00 PM.");
            return View(model);
        }

        // Optional: Validate day of week (Saturday to Thursday)
        var dayOfWeek = model.PreferredDate.Value.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Friday)
        {
            ModelState.AddModelError("PreferredDate", "Screenings are not available on Fridays. Please choose another day (Saturday to Thursday).");
            return View(model);
        }

        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var scheduledDateTime = model.PreferredDate.Value.Date + selectedTime;
        var endTime = scheduledDateTime.AddHours(1);

        var screening = new ScreeningAppointment
        {
            ApplicationId = model.ApplicationId,
            UnitId = model.UnitId,
            TenantId = appUser.UserId,
            ScheduledDate = scheduledDateTime,
            EndTime = endTime,
            Status = "Pending",
            Notes = model.Notes,
            CreatedAt = DateTime.Now
        };

        _db.ScreeningAppointments.Add(screening);
        await _db.SaveChangesAsync();

        // Update application status to "Screening" if not already
        if (application.Status != "Screening")
        {
            application.Status = "Screening";
            await _db.SaveChangesAsync();
        }

        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
        {
            await _notifier.SendAsync(mgr.UserId,
                $"New screening request for Unit {model.UnitNumber} by {appUser.FullName} on {scheduledDateTime:dd MMM yyyy HH:mm}",
                "LeaseUpdate");
        }

        TempData["Success"] = "Screening appointment requested! You will be notified once confirmed.";
        return RedirectToAction("MyScreenings");
    }
    // GET: /Screening/MyScreenings (with optional status filter)
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> MyScreenings(string? status)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var query = _db.ScreeningAppointments
            .Include(s => s.Unit).ThenInclude(u => u.Property)
            .Where(s => s.TenantId == appUser.UserId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(s => s.Status == status);

        var screenings = await query
            .OrderByDescending(s => s.ScheduledDate)
            .Select(s => new ScreeningListViewModel
            {
                ScreeningId = s.ScreeningId,
                ApplicationId = s.ApplicationId,
                UnitNumber = s.Unit.UnitNumber,
                PropertyName = s.Unit.Property.Name,
                TenantName = appUser.FullName,
                ScheduledDate = s.ScheduledDate,
                EndTime = s.EndTime,
                Status = s.Status,
                Notes = s.Notes,
                ManagerNotes = s.ManagerNotes,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();

        ViewBag.Status = status;
        return View(screenings);
    }

    // GET: /Screening/Manage (Manager only)
    [Authorize(Roles = "PropertyManager")]
    public async Task<IActionResult> Manage(string? status)
    {
        var query = _db.ScreeningAppointments
            .Include(s => s.Unit).ThenInclude(u => u.Property)
            .Include(s => s.Tenant)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(s => s.Status == status);

        var screenings = await query
            .OrderBy(s => s.ScheduledDate)
            .Select(s => new ScreeningListViewModel
            {
                ScreeningId = s.ScreeningId,
                ApplicationId = s.ApplicationId,
                UnitNumber = s.Unit.UnitNumber,
                PropertyName = s.Unit.Property.Name,
                TenantName = s.Tenant.FullName,
                ScheduledDate = s.ScheduledDate,
                EndTime = s.EndTime,
                Status = s.Status,
                Notes = s.Notes,
                ManagerNotes = s.ManagerNotes,
                CreatedAt = s.CreatedAt
            })
            .ToListAsync();

        ViewBag.Status = status;
        return View(screenings);
    }

    // GET: /Screening/ManageScreening/{id}
    [Authorize(Roles = "PropertyManager")]
    public async Task<IActionResult> ManageScreening(int id)
    {
        var screening = await _db.ScreeningAppointments
            .Include(s => s.Unit).ThenInclude(u => u.Property)
            .Include(s => s.Tenant)
            .FirstOrDefaultAsync(s => s.ScreeningId == id);

        if (screening == null) return NotFound();

        var model = new ManageScreeningViewModel
        {
            ScreeningId = screening.ScreeningId,
            ApplicationId = screening.ApplicationId,
            UnitNumber = screening.Unit.UnitNumber,
            PropertyName = screening.Unit.Property.Name,
            TenantName = screening.Tenant.FullName,
            TenantPhone = screening.Tenant.Phone ?? "N/A",
            TenantEmail = screening.Tenant.Email,
            ScheduledDate = screening.ScheduledDate,
            EndTime = screening.EndTime,
            CurrentStatus = screening.Status,
            Notes = screening.Notes,
            ManagerNotes = screening.ManagerNotes,
            NewStatus = screening.Status
        };
        return View(model);
    }

    // POST: /Screening/ManageScreening
    [Authorize(Roles = "PropertyManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ManageScreening(ManageScreeningViewModel model)
    {
        var screening = await _db.ScreeningAppointments
            .Include(s => s.Tenant)
            .Include(s => s.Unit).ThenInclude(u => u.Property)
            .FirstOrDefaultAsync(s => s.ScreeningId == model.ScreeningId);

        if (screening == null) return NotFound();

        screening.Status = model.NewStatus;
        screening.ManagerNotes = model.ManagerNotes;
        screening.UpdatedAt = DateTime.Now;

        if (model.NewStatus == "Rescheduled" && model.RescheduleDate.HasValue && !string.IsNullOrEmpty(model.RescheduleTime))
        {
            var newDateTime = model.RescheduleDate.Value.Date + TimeSpan.Parse(model.RescheduleTime);
            screening.ScheduledDate = newDateTime;
            screening.EndTime = newDateTime.AddHours(1);
        }

        await _db.SaveChangesAsync();

        var message = $"Your screening for Unit {screening.Unit.UnitNumber} has been {model.NewStatus.ToLower()}.";
        if (model.NewStatus == "Confirmed")
            message = $"Your screening for Unit {screening.Unit.UnitNumber} is confirmed for {screening.ScheduledDate:dd MMM yyyy HH:mm}. Please arrive on time.";
        else if (model.NewStatus == "Rescheduled")
            message = $"Your screening for Unit {screening.Unit.UnitNumber} has been rescheduled to {screening.ScheduledDate:dd MMM yyyy HH:mm}.";

        await _notifier.SendAsync(screening.TenantId, message, "LeaseUpdate");

        if (model.NewStatus == "Completed")
        {
            TempData["Success"] = "Screening marked as completed. You can now create the lease agreement.";
            return RedirectToAction("Create", "LeaseAgreements", new { screeningId = screening.ScreeningId });
        }

        TempData["Success"] = $"Screening status updated to {model.NewStatus}.";
        return RedirectToAction("Manage");
    }

    // ======================== EDIT AND CANCEL ========================

    // GET: /Screening/Edit/{id}
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> Edit(int id)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var screening = await _db.ScreeningAppointments
            .Include(s => s.Unit)
            .ThenInclude(u => u.Property)
            .Include(s => s.Application)
            .FirstOrDefaultAsync(s => s.ScreeningId == id && s.TenantId == appUser.UserId);

        if (screening == null) return NotFound();

        if (screening.Status != "Pending" && screening.Status != "Confirmed")
        {
            TempData["Error"] = "This screening cannot be edited.";
            return RedirectToAction("MyScreenings");
        }

        var model = new EditScreeningViewModel
        {
            ScreeningId = screening.ScreeningId,
            ApplicationId = screening.ApplicationId,
            UnitNumber = screening.Unit.UnitNumber,
            PropertyName = screening.Unit.Property?.Name ?? "",
            PreferredDate = screening.ScheduledDate.Date,
            PreferredTime = screening.ScheduledDate.ToString("HH:mm"),
            Notes = screening.Notes
        };
        return View(model);
    }

    // POST: /Screening/Edit
    [Authorize(Roles = "Tenant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditScreeningViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var screening = await _db.ScreeningAppointments
            .Include(s => s.Unit)
            .FirstOrDefaultAsync(s => s.ScreeningId == model.ScreeningId);

        if (screening == null) return NotFound();

        var newDateTime = model.PreferredDate.Date + TimeSpan.Parse(model.PreferredTime);
        screening.ScheduledDate = newDateTime;
        screening.EndTime = newDateTime.AddHours(1);
        screening.Notes = model.Notes;
        screening.Status = "Pending";
        screening.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
        {
            await _notifier.SendAsync(mgr.UserId,
                $"Screening appointment for Unit {screening.Unit.UnitNumber} has been rescheduled to {newDateTime:dd MMM yyyy HH:mm}.",
                "LeaseUpdate");
        }

        TempData["Success"] = "Screening appointment updated. Waiting for manager confirmation.";
        return RedirectToAction("MyScreenings");
    }

    // POST: /Screening/Cancel
    [Authorize(Roles = "Tenant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int screeningId)
    {
        var screening = await _db.ScreeningAppointments
            .Include(s => s.Unit)
            .Include(s => s.Application)
            .FirstOrDefaultAsync(s => s.ScreeningId == screeningId);

        if (screening == null) return NotFound();

        screening.Status = "Cancelled";
        screening.Application.Status = "Rejected";
        screening.Application.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        await _notifier.SendAsync(screening.TenantId,
            $"You have cancelled the screening for Unit {screening.Unit.UnitNumber}. Your lease application has been rejected.",
            "LeaseUpdate");

        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
        {
            await _notifier.SendAsync(mgr.UserId,
                $"Tenant cancelled screening for Unit {screening.Unit.UnitNumber}. Application rejected.",
                "LeaseUpdate");
        }

        TempData["Success"] = "Screening cancelled. Application rejected.";
        return RedirectToAction("MyScreenings");
    }
}