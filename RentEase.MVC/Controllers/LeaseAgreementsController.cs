using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentEase.API.Data;
using RentEase.API.Models;
using RentEase.MVC.Services;
using RentEase.MVC.ViewModels;

namespace RentEase.MVC.Controllers;

[Authorize(Roles = "PropertyManager")]
public class LeaseAgreementsController : Controller
{
    private readonly PropertyLeasingDbContext _db;
    private readonly UserManager<AppUser> _userManager;
    private readonly NotificationService _notifier;

    public LeaseAgreementsController(
        PropertyLeasingDbContext db,
        UserManager<AppUser> userManager,
        NotificationService notifier)
    {
        _db = db;
        _userManager = userManager;
        _notifier = notifier;
    }

    // GET: /LeaseAgreements/Create/{screeningId}
    public async Task<IActionResult> Create(int screeningId)
    {
        var screening = await _db.ScreeningAppointments
            .Include(s => s.Unit)
            .ThenInclude(u => u.Property)
            .Include(s => s.Tenant)
            .Include(s => s.Application)
            .FirstOrDefaultAsync(s => s.ScreeningId == screeningId);

        if (screening == null) return NotFound();

        if (screening.Status != "Completed")
        {
            TempData["Error"] = "Screening must be completed before creating a lease agreement.";
            return RedirectToAction("Manage", "Screening");
        }

        // Check if agreement already exists
        var existingAgreement = await _db.LeaseAgreements
            .FirstOrDefaultAsync(l => l.ScreeningId == screeningId);

        if (existingAgreement != null)
        {
            TempData["Error"] = "A lease agreement already exists for this screening.";
            return RedirectToAction("Details", new { id = existingAgreement.LeaseAgreementId });
        }

        var model = new CreateLeaseAgreementViewModel
        {
            ApplicationId = screening.ApplicationId,
            ScreeningId = screening.ScreeningId,
            UnitId = screening.UnitId,
            UnitNumber = screening.Unit.UnitNumber,
            PropertyName = screening.Unit.Property.Name,
            TenantName = screening.Tenant.FullName,
            TenantId = screening.TenantId,
            MonthlyRent = screening.Unit.MonthlyRent ?? 0,
            SecurityDeposit = (screening.Unit.MonthlyRent ?? 0) * 2,
            LateFeePerDay = 5.00m,
            TermsAndConditions = "Standard lease terms apply. Tenant agrees to maintain the property in good condition and pay rent on time.",
            LeaseStartDate = DateTime.Now.AddDays(7),
            LeaseEndDate = DateTime.Now.AddYears(1).AddDays(7)
        };

        return View(model);
    }

    // POST: /LeaseAgreements/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLeaseAgreementViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var screening = await _db.ScreeningAppointments
            .Include(s => s.Unit)
            .FirstOrDefaultAsync(s => s.ScreeningId == model.ScreeningId);

        if (screening == null) return NotFound();

        var agreement = new LeaseAgreement
        {
            ApplicationId = model.ApplicationId,
            ScreeningId = model.ScreeningId,
            UnitId = model.UnitId,
            TenantId = model.TenantId,
            LeaseStartDate = model.LeaseStartDate,
            LeaseEndDate = model.LeaseEndDate,
            MonthlyRent = model.MonthlyRent,
            SecurityDeposit = model.SecurityDeposit,
            LateFeePerDay = model.LateFeePerDay,
            TermsAndConditions = model.TermsAndConditions,
            SpecialClauses = model.SpecialClauses,
            Status = "Draft",
            CreatedAt = DateTime.Now
        };

        _db.LeaseAgreements.Add(agreement);
        await _db.SaveChangesAsync();

        // Notify tenant
        await _notifier.SendAsync(model.TenantId,
            $"A lease agreement for Unit {model.UnitNumber} is ready for your review. Please login to view and sign.",
            "LeaseUpdate");

        TempData["Success"] = "Lease agreement created successfully!";
        return RedirectToAction("Details", new { id = agreement.LeaseAgreementId });
    }

    // GET: /LeaseAgreements/Details/{id}
    public async Task<IActionResult> Details(int id)
    {
        var agreement = await _db.LeaseAgreements
            .Include(l => l.Unit)
            .ThenInclude(u => u.Property)
            .Include(l => l.Tenant)
            .Include(l => l.Screening)
            .FirstOrDefaultAsync(l => l.LeaseAgreementId == id);

        if (agreement == null) return NotFound();

        // Check if user is tenant or manager
        var identityUser = await _userManager.GetUserAsync(User);
        var appUser = await _db.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser!.Id);

        if (appUser == null) return Unauthorized();

        if (appUser.Role == "Tenant" && agreement.TenantId != appUser.UserId)
        {
            return Unauthorized();
        }

        var model = new LeaseAgreementViewModel
        {
            LeaseAgreementId = agreement.LeaseAgreementId,
            ApplicationId = agreement.ApplicationId,
            ScreeningId = agreement.ScreeningId,
            UnitNumber = agreement.Unit.UnitNumber,
            PropertyName = agreement.Unit.Property.Name,
            PropertyAddress = agreement.Unit.Property.Address,
            TenantName = agreement.Tenant.FullName,
            TenantPhone = agreement.Tenant.Phone ?? "N/A",
            TenantEmail = agreement.Tenant.Email,
            LeaseStartDate = agreement.LeaseStartDate,
            LeaseEndDate = agreement.LeaseEndDate,
            MonthlyRent = agreement.MonthlyRent,
            SecurityDeposit = agreement.SecurityDeposit,
            LateFeePerDay = agreement.LateFeePerDay,
            Status = agreement.Status,
            TermsAndConditions = agreement.TermsAndConditions,
            SpecialClauses = agreement.SpecialClauses,
            SignedDate = agreement.SignedDate,
            CreatedAt = agreement.CreatedAt,
            ScreeningDate = agreement.Screening?.ScheduledDate
        };

        return View(model);
    }

    // POST: /LeaseAgreements/Sign/{id}
    [Authorize(Roles = "Tenant")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sign(int id)
    {
        var agreement = await _db.LeaseAgreements
            .Include(l => l.Tenant)
            .Include(l => l.Unit)
            .FirstOrDefaultAsync(l => l.LeaseAgreementId == id);

        if (agreement == null) return NotFound();

        var identityUser = await _userManager.GetUserAsync(User);
        var appUser = await _db.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser!.Id);

        if (appUser == null || agreement.TenantId != appUser.UserId)
            return Unauthorized();

        agreement.Status = "Signed";
        agreement.SignedDate = DateTime.Now;
        agreement.UpdatedAt = DateTime.Now;
        await _db.SaveChangesAsync();

        // Create actual lease record
        var lease = new Lease
        {
            ApplicationId = agreement.ApplicationId,
            LeaseStartDate = agreement.LeaseStartDate,
            LeaseEndDate = agreement.LeaseEndDate,
            MonthlyRent = agreement.MonthlyRent,
            SecurityDeposit = agreement.SecurityDeposit,
            CreatedAt = DateTime.Now
        };
        _db.Leases.Add(lease);
        await _db.SaveChangesAsync();

        // Update unit status
        var unit = await _db.Units.FindAsync(agreement.UnitId);
        if (unit != null)
        {
            unit.AvailabilityStatus = "Occupied";
            await _db.SaveChangesAsync();
        }

        // Notify manager
        var managers = await _db.Users.Where(u => u.Role == "PropertyManager").ToListAsync();
        foreach (var mgr in managers)
        {
            await _notifier.SendAsync(mgr.UserId,
                $"Tenant {agreement.Tenant.FullName} has signed the lease agreement for Unit {agreement.Unit.UnitNumber}.",
                "LeaseUpdate");
        }

        TempData["Success"] = "Lease agreement signed successfully! Welcome to your new home.";
        return RedirectToAction("Details", new { id });
    }

    // GET: /LeaseAgreements/MyLeases (Tenant view)
    [Authorize(Roles = "Tenant")]
    public async Task<IActionResult> MyLeases()
    {
        var identityUser = await _userManager.GetUserAsync(User);
        var appUser = await _db.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser!.Id);

        if (appUser == null) return Unauthorized();

        var agreements = await _db.LeaseAgreements
            .Include(l => l.Unit)
            .ThenInclude(u => u.Property)
            .Where(l => l.TenantId == appUser.UserId)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new LeaseAgreementListViewModel
            {
                LeaseAgreementId = l.LeaseAgreementId,
                UnitNumber = l.Unit.UnitNumber,
                PropertyName = l.Unit.Property.Name,
                TenantName = l.Tenant.FullName,
                LeaseStartDate = l.LeaseStartDate,
                LeaseEndDate = l.LeaseEndDate,
                MonthlyRent = l.MonthlyRent,
                Status = l.Status,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync();

        return View(agreements);
    }

    // GET: /LeaseAgreements/All (Manager view)
    [Authorize(Roles = "PropertyManager")]
    public async Task<IActionResult> All(string? status)
    {
        var query = _db.LeaseAgreements
            .Include(l => l.Unit)
            .ThenInclude(u => u.Property)
            .Include(l => l.Tenant)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(l => l.Status == status);

        var agreements = await query
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new LeaseAgreementListViewModel
            {
                LeaseAgreementId = l.LeaseAgreementId,
                UnitNumber = l.Unit.UnitNumber,
                PropertyName = l.Unit.Property.Name,
                TenantName = l.Tenant.FullName,
                LeaseStartDate = l.LeaseStartDate,
                LeaseEndDate = l.LeaseEndDate,
                MonthlyRent = l.MonthlyRent,
                Status = l.Status,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync();

        ViewBag.Status = status;
        return View(agreements);
    }
}