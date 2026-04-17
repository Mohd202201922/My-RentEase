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
public class MaintenanceController : Controller
{
    private readonly PropertyLeasingDbContext _db;
    private readonly UserManager<AppUser>     _userManager;
    private readonly NotificationService      _notifier;

    public MaintenanceController(
        PropertyLeasingDbContext db,
        UserManager<AppUser> userManager,
        NotificationService notifier)
    {
        _db          = db;
        _userManager = userManager;
        _notifier    = notifier;
    }

    private async Task<User?> GetAppUserAsync()
    {
        var identity = await _userManager.GetUserAsync(User);
        if (identity == null) return null;
        return await _db.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identity.Id);
    }

    // GET /Maintenance
    public async Task<IActionResult> Index(string? status, string? priority)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var query = _db.MaintenanceRequests
            .Include(r => r.Unit).ThenInclude(u => u.Property)
            .Include(r => r.Tenant)
            .Include(r => r.AssignedStaff)
            .Include(r => r.Status)
            .AsQueryable();

        // Tenants only see their own requests
        if (appUser.Role == "Tenant")
            query = query.Where(r => r.TenantUserId == appUser.UserId);

        // Staff only see requests assigned to them
        if (appUser.Role == "MaintenanceStaff")
            query = query.Where(r => r.AssignedStaffId == appUser.UserId);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(r => r.Status != null && r.Status.StatusName == status);

        if (!string.IsNullOrWhiteSpace(priority))
            query = query.Where(r => r.Priority == priority);

        var requests = await query
            .OrderByDescending(r => r.SubmittedAt)
            .Select(r => new MaintenanceListViewModel
            {
                RequestId     = r.RequestId,
                Title         = r.Title,
                Description   = r.Description,
                RequestType   = r.RequestType,
                Priority      = r.Priority,
                Status        = r.Status != null ? r.Status.StatusName : "Unknown",
                TicketNumber  = r.TicketNumber,
                UnitNumber    = r.Unit.UnitNumber,
                PropertyName  = r.Unit.Property.Name,
                TenantName    = r.Tenant.FullName,
                AssignedStaff = r.AssignedStaff != null ? r.AssignedStaff.FullName : null,
                SubmittedAt   = r.SubmittedAt,
                ResolvedAt    = r.ResolvedAt
            })
            .ToListAsync();

        ViewBag.Status   = status;
        ViewBag.Priority = priority;
        return View(requests);
    }

    // GET /Maintenance/Submit
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> Submit(int? unitId)
    {
        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        // Pre-populate unit if tenant has active lease
        Unit? unit = null;
        if (unitId.HasValue)
        {
            unit = await _db.Units.Include(u => u.Property).FirstOrDefaultAsync(u => u.UnitId == unitId);
        }
        else
        {
            // Find tenant's active lease unit
            var lease = await _db.Leases
                .Include(l => l.Application).ThenInclude(a => a.Unit).ThenInclude(u => u.Property)
                .Include(l => l.StatusHistory)
                    .ThenInclude(h => h.Status)
                .Where(l => l.Application.UserId == appUser.UserId
                    && l.StatusHistory.Any(h => h.IsCurrent && h.Status.StatusName == "Active"))
                .FirstOrDefaultAsync();
            unit = lease?.Application.Unit;
        }

        return View(new CreateMaintenanceViewModel
        {
            UnitId       = unit?.UnitId ?? 0,
            UnitNumber   = unit?.UnitNumber ?? "",
            PropertyName = unit?.Property.Name ?? ""
        });
    }

    // POST /Maintenance/Submit
    [Authorize(Roles = "Tenant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(CreateMaintenanceViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var appUser = await GetAppUserAsync();
        if (appUser == null) return Unauthorized();

        var ticketNumber = $"TKT-{DateTime.Now:yyyy}-{new Random().Next(1000, 9999)}";
        var submittedStatusId = await _db.MaintenanceRequestStatuses
            .Where(s => s.StatusName == "Submitted")
            .Select(s => (int?)s.StatusId)
            .FirstOrDefaultAsync();
        if (!submittedStatusId.HasValue)
        {
            TempData["Error"] = "Maintenance statuses are missing from the database.";
            return View(model);
        }

        var request = new MaintenanceRequest
        {
            UnitId       = model.UnitId,
            TenantUserId = appUser.UserId,
            Title        = model.Title,
            Description  = model.Description,
            RequestType  = model.RequestType,
            Priority     = model.Priority,
            StatusId     = submittedStatusId.Value,
            TicketNumber = ticketNumber,
            SubmittedAt  = DateTime.Now
        };

        _db.MaintenanceRequests.Add(request);
        await _db.SaveChangesAsync();

        // Notify managers
        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
            await _notifier.SendAsync(mgr.UserId,
                $"New maintenance request: {model.Title} — Ticket: {ticketNumber}",
                "MaintenanceUpdate");

        TempData["Success"] = $"Request submitted! Your ticket number is {ticketNumber}.";
        return RedirectToAction("Index");
    }

    // GET /Maintenance/Update/{id} — Manager/Staff
    [Authorize(Roles = "PropertyManager,MaintenanceStaff")]
    public async Task<IActionResult> Update(int id)
    {
        var request = await _db.MaintenanceRequests
            .Include(r => r.Status)
            .FirstOrDefaultAsync(r => r.RequestId == id);
        if (request == null) return NotFound();

        var staffList = await _db.Users
            .Where(u => u.Role == "MaintenanceStaff")
            .Select(u => new StaffSelectItem { UserId = u.UserId, FullName = u.FullName })
            .ToListAsync();

        return View(new UpdateMaintenanceViewModel
        {
            RequestId     = request.RequestId,
            Title         = request.Title,
            CurrentStatus = request.Status?.StatusName ?? "Unknown",
            NewStatus     = request.Status?.StatusName ?? "Unknown",
            StaffList     = staffList
        });
    }

    // POST /Maintenance/Update
    [Authorize(Roles = "PropertyManager,MaintenanceStaff")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateMaintenanceViewModel model)
    {
        var request = await _db.MaintenanceRequests
            .Include(r => r.Tenant)
            .Include(r => r.Status)
            .FirstOrDefaultAsync(r => r.RequestId == model.RequestId);

        if (request == null) return NotFound();

        var appUser = await GetAppUserAsync();
        var newStatus = await _db.MaintenanceRequestStatuses
            .FirstOrDefaultAsync(s => s.StatusName == model.NewStatus);
        if (newStatus == null)
        {
            TempData["Error"] = $"Unknown maintenance status: {model.NewStatus}.";
            return RedirectToAction("Update", new { id = model.RequestId });
        }

        // Save status history
        _db.MaintenanceStatusHistories.Add(new MaintenanceStatusHistory
        {
            RequestId       = request.RequestId,
            OldStatus       = request.Status?.StatusName,
            NewStatus       = model.NewStatus,
            Notes           = model.Notes,
            ChangedAt       = DateTime.Now,
            ChangedByUserId = appUser?.UserId
        });

        request.StatusId = newStatus.StatusId;

        if (model.AssignedStaffId.HasValue)
            request.AssignedStaffId = model.AssignedStaffId;

        if (model.NewStatus == "Resolved")
        {
            request.ResolvedAt      = DateTime.Now;
            request.ResolutionNotes = model.Notes;
        }

        await _db.SaveChangesAsync();

        // Notify tenant
        await _notifier.SendAsync(request.TenantUserId,
            $"Your maintenance request \"{request.Title}\" status changed to: {model.NewStatus}.",
            "MaintenanceUpdate");

        // Notify assigned staff if newly assigned
        if (model.AssignedStaffId.HasValue)
            await _notifier.SendAsync(model.AssignedStaffId.Value,
                $"You have been assigned to maintenance request: {request.Title} (Ticket: {request.TicketNumber})",
                "MaintenanceUpdate");

        TempData["Success"] = "Maintenance request updated successfully.";
        return RedirectToAction("Index");
    }
}
