using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentEase.API.Data;
using RentEase.API.Models;
using RentEase.MVC.ViewModels;

namespace RentEase.MVC.Controllers;

public class PropertiesController : Controller
{
    private readonly PropertyLeasingDbContext _db;

    public PropertiesController(PropertyLeasingDbContext db)
    {
        _db = db;
    }

    // GET /Properties — all properties (public)
    public async Task<IActionResult> Index(string? search, string? type)
    {
        var query = _db.Properties
            .Include(p => p.Location)   // NEW
            .Include(p => p.Units)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search) ||
                                     p.Location.Street.Contains(search) ||
                                     p.Location.City.Contains(search));

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(p => p.PropertyType == type);

        var properties = await query
    .Select(p => new PropertyListViewModel
    {
        PropertyId = p.PropertyId,
        Name = p.Name,
        Description = p.Description,
        BuildingNumber = p.Location == null ? null : p.Location.BuildingNumber,
        Block = p.Location == null ? null : p.Location.Block,
        Street = p.Location == null ? null : p.Location.Street,
        City = p.Location == null ? "" : p.Location.City,
        LocationUrl = p.Location == null ? null : p.Location.LocationUrl,
        PropertyType = p.PropertyType,
        ImgPath = p.ImgPath,
        TotalUnits = p.Units.Count,
        AvailableUnits = p.Units.Count(u => u.AvailabilityStatus == "Available")
    })
    .ToListAsync();


        ViewBag.Search = search;
        ViewBag.Type = type;
        return View(properties);
    }

    // GET /Properties/Units/{propertyId}
    public async Task<IActionResult> Units(int propertyId, string? unitType, decimal? maxRent)
    {
        var property = await _db.Properties
            .Include(p => p.Location)
            .FirstOrDefaultAsync(p => p.PropertyId == propertyId);
        if (property == null) return NotFound();

        var query = _db.Units
            .Include(u => u.Property).ThenInclude(p => p.Location)
            .Include(u => u.Feedbacks)
            .Where(u => u.PropertyId == propertyId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(unitType))
            query = query.Where(u => u.UnitType == unitType);

        if (maxRent.HasValue)
            query = query.Where(u => u.MonthlyRent <= maxRent);

        var units = await query
            .Select(u => new UnitListViewModel
            {
                UnitId = u.UnitId,
                UnitNumber = u.UnitNumber,
                UnitType = u.UnitType,
                Sizesqm = u.Sizesqm,
                MonthlyRent = u.MonthlyRent,
                Amenities = u.Amenities,
                AvailabilityStatus = u.AvailabilityStatus,
                ImgPath = u.ImgPath,
                PropertyName = u.Property.Name,
                PropertyAddress = $"{u.Property.Location.BuildingNumber} {u.Property.Location.Street}, {u.Property.Location.City}",
                PropertyId = u.PropertyId,
                AverageRating = u.Feedbacks.Any() ? u.Feedbacks.Average(f => (double)(f.Rating ?? 0)) : 0,
                FeedbackCount = u.Feedbacks.Count(f => f.IsVisible)
            })
            .ToListAsync();

        ViewBag.PropertyName = property.Name;
        ViewBag.PropertyId = propertyId;
        ViewBag.UnitType = unitType;
        ViewBag.MaxRent = maxRent;
        return View(units);
    }

    // GET /Properties/UnitDetails/{id}
    public async Task<IActionResult> UnitDetails(int id)
    {
        var unit = await _db.Units
            .Include(u => u.Property)
                .ThenInclude(p => p.Location)
            .Include(u => u.Feedbacks.Where(f => f.IsVisible))
                .ThenInclude(f => f.User)
            .FirstOrDefaultAsync(u => u.UnitId == id);

        if (unit == null) return NotFound();
        return View(unit);
    }

    // ── Manager only: Manage Properties ────────────────

    // GET /Properties/Manage
    [Authorize(Roles = "PropertyManager")]
    public async Task<IActionResult> Manage()
    {
        var properties = await _db.Properties
            .Include(p => p.Location)
            .Include(p => p.Units)
            .ToListAsync();
        return View(properties);
    }

    // GET /Properties/Create
    [Authorize(Roles = "PropertyManager")]
    public IActionResult Create() => View();

    // POST /Properties/Create
    [Authorize(Roles = "PropertyManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PropertyViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // Create Location first
        var location = new Location
        {
            BuildingNumber = model.BuildingNumber,
            Block = model.Block,
            Street = model.Street,
            City = model.City,
            LocationUrl = model.LocationUrl
        };
        _db.Locations.Add(location);
        await _db.SaveChangesAsync();

        // Create Property linked to Location
        var property = new Property
        {
            Name = model.Name,
            Description = model.Description,
            PropertyType = model.PropertyType,
            ImgPath = model.ImgPath,
            LocationId = location.LocationId
        };
        _db.Properties.Add(property);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Property created successfully.";
        return RedirectToAction("Manage");
    }

    // GET /Properties/Edit/{id}
    [Authorize(Roles = "PropertyManager")]
    public async Task<IActionResult> Edit(int id)
    {
        var property = await _db.Properties
            .Include(p => p.Location)
            .FirstOrDefaultAsync(p => p.PropertyId == id);
        if (property == null) return NotFound();

        var model = new PropertyViewModel
        {
            PropertyId = property.PropertyId,
            Name = property.Name,
            Description = property.Description,
            PropertyType = property.PropertyType,
            ImgPath = property.ImgPath,
            BuildingNumber = property.Location.BuildingNumber,
            Block = property.Location.Block,
            Street = property.Location.Street,
            City = property.Location.City,
            LocationUrl = property.Location.LocationUrl
        };
        return View(model);
    }

    // POST /Properties/Edit/{id}
    [Authorize(Roles = "PropertyManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PropertyViewModel model)
    {
        if (id != model.PropertyId) return BadRequest();
        if (!ModelState.IsValid) return View(model);

        var property = await _db.Properties
            .Include(p => p.Location)
            .FirstOrDefaultAsync(p => p.PropertyId == id);
        if (property == null) return NotFound();

        // Update Property
        property.Name = model.Name;
        property.Description = model.Description;
        property.PropertyType = model.PropertyType;
        property.ImgPath = model.ImgPath;

        // Update Location
        property.Location.BuildingNumber = model.BuildingNumber;
        property.Location.Block = model.Block;
        property.Location.Street = model.Street;
        property.Location.City = model.City;
        property.Location.LocationUrl = model.LocationUrl;

        await _db.SaveChangesAsync();

        TempData["Success"] = "Property updated successfully.";
        return RedirectToAction("Manage");
    }

    // POST /Properties/Delete/{id}
    [Authorize(Roles = "PropertyManager")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var property = await _db.Properties
            .Include(p => p.Location)
            .FirstOrDefaultAsync(p => p.PropertyId == id);
        if (property == null) return NotFound();

        // Location will be cascade deleted (FK)
        _db.Properties.Remove(property);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Property deleted.";
        return RedirectToAction("Manage");
    }
}