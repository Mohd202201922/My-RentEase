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
            .Include(a => a.Unit)
            .ThenInclude(u => u.Property)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId && a.UserId == appUser.UserId);

        if (application == null) return NotFound();

        // Check if already has a screening
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
            MonthlyRent = application.Unit.MonthlyRent
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

        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        // Parse the date and time
        var scheduledDateTime = model.PreferredDate.Date + TimeSpan.Parse(model.PreferredTime);
        var endTime = scheduledDateTime.AddHours(1); // 1 hour appointment

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

        // Update application status to "Screening"
        var application = await _db.LeaseApplications.FindAsync(model.ApplicationId);
        if (application != null)
        {
            application.Status = "Screening";
            await _db.SaveChangesAsync();
        }

        // Notify all managers
        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
        {
            await _notifier.SendAsync(mgr.UserId,
                $"New screening request for Unit {model.UnitNumber} by {appUser.FullName} on {scheduledDateTime:dd MMM yyyy HH:mm}",
                "LeaseUpdate");
        }

        TempData["Success"] = $"Screening appointment requested! You will be notified once confirmed.";
        return RedirectToAction("MyScreenings");
    }

    // GET: /Screening/MyScreenings
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> MyScreenings()
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var screenings = await _db.ScreeningAppointments
            .Include(s => s.Unit)
            .ThenInclude(u => u.Property)
            .Where(s => s.TenantId == appUser.UserId)
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

        return View(screenings);
    }

    // GET: /Screening/Manage (Manager only)
    [Authorize(Roles = "PropertyManager")]
    public async Task<IActionResult> Manage(string? status)
    {
        var query = _db.ScreeningAppointments
            .Include(s => s.Unit)
            .ThenInclude(u => u.Property)
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
            .Include(s => s.Unit)
            .ThenInclude(u => u.Property)
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
            .Include(s => s.Unit)
            .ThenInclude(u => u.Property)
            .FirstOrDefaultAsync(s => s.ScreeningId == model.ScreeningId);

        if (screening == null) return NotFound();

        var oldStatus = screening.Status;
        screening.Status = model.NewStatus;
        screening.ManagerNotes = model.ManagerNotes;
        screening.UpdatedAt = DateTime.Now;

        // Handle rescheduling
        if (model.NewStatus == "Rescheduled" && model.RescheduleDate.HasValue && !string.IsNullOrEmpty(model.RescheduleTime))
        {
            var newDateTime = model.RescheduleDate.Value.Date + TimeSpan.Parse(model.RescheduleTime);
            screening.ScheduledDate = newDateTime;
            screening.EndTime = newDateTime.AddHours(1);
        }

        await _db.SaveChangesAsync();

        // Notify tenant
        var message = $"Your screening for Unit {screening.Unit.UnitNumber} at {screening.Unit.Property.Name} has been {model.NewStatus.ToLower()}.";
        if (model.NewStatus == "Confirmed")
        {
            message = $"Your screening for Unit {screening.Unit.UnitNumber} is confirmed for {screening.ScheduledDate:dd MMM yyyy HH:mm}. Please arrive on time.";
        }
        else if (model.NewStatus == "Rescheduled")
        {
            message = $"Your screening for Unit {screening.Unit.UnitNumber} has been rescheduled to {screening.ScheduledDate:dd MMM yyyy HH:mm}.";
        }

        await _notifier.SendAsync(screening.TenantId, message, "LeaseUpdate");

        // If screening is completed, allow creating lease agreement
        if (model.NewStatus == "Completed")
        {
            TempData["Success"] = $"Screening marked as completed. You can now create the lease agreement.";
            return RedirectToAction("Create", "LeaseAgreements", new { screeningId = screening.ScreeningId });
        }

        TempData["Success"] = $"Screening status updated to {model.NewStatus}.";
        return RedirectToAction("Manage");
    }
}