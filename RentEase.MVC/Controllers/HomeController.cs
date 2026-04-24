using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentEase.API.Data;
using RentEase.API.Models;
using RentEase.MVC.Services;
using RentEase.MVC.ViewModels;

namespace RentEase.MVC.Controllers;

public class HomeController : Controller
{
    private readonly PropertyLeasingDbContext _db;
    private readonly ApiService _apiService;
    private readonly UserManager<AppUser> _userManager;

    public HomeController(PropertyLeasingDbContext db, ApiService apiService, UserManager<AppUser> userManager)
    {
        _db = db;
        _apiService = apiService;
        _userManager = userManager;
    }

    // GET / — redirect Property Manager to Dashboard
    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && await _userManager.IsInRoleAsync(user, "PropertyManager"))
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }

        // Show stats on landing page for non-managers
        ViewBag.TotalProperties = await _db.Properties.CountAsync();
        ViewBag.AvailableUnits = await _db.Units.CountAsync(u => u.AvailabilityStatus == "Available");
        ViewBag.TotalUnits = await _db.Units.CountAsync();
        ViewBag.FeaturedProperties = await _db.Properties
            .Include(p => p.Location)   // NEW: include location
            .Include(p => p.Units)
            .Take(3)
            .ToListAsync();
        return View();
    }

    // GET /Home/MaintenanceLookup — public page
    public IActionResult MaintenanceLookup()
    {
        return View(new PublicLookupViewModel());
    }

    // POST /Home/MaintenanceLookup — calls API via HttpClient
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MaintenanceLookup(PublicLookupViewModel model)
    {
        model.Searched = true;

        if (!ModelState.IsValid)
            return View(model);

        var result = await _apiService.LookupMaintenanceTicketAsync(
            model.TicketNumber!, model.Phone!);

        if (result == null)
        {
            model.ErrorMessage = "No maintenance request found with the provided ticket number and phone. Please check your details and try again.";
        }
        else
        {
            model.Result = result;
        }

        return View(model);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}