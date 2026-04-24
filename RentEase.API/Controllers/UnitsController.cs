using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentEase.API.Data;
using RentEase.API.DTOs;
using RentEase.API.Models;

namespace RentEase.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnitsController : ControllerBase
{
    private readonly PropertyLeasingDbContext _db;

    public UnitsController(PropertyLeasingDbContext db)
    {
        _db = db;
    }

    // GET api/units — public, all available units
    [HttpGet]
    public async Task<IActionResult> GetAvailableUnits()
    {
        var units = await _db.Units
            .Include(u => u.Property)
                .ThenInclude(p => p.Location)  // NEW: include Location
            .Where(u => u.AvailabilityStatus == "Available")
            .Select(u => new UnitDto
            {
                UnitId = u.UnitId,
                UnitNumber = u.UnitNumber,
                UnitType = u.UnitType,
                Sizesqm = u.Sizesqm,
                MonthlyRent = u.MonthlyRent,
                Amenities = u.Amenities,
                AvailabilityStatus = u.AvailabilityStatus,
                PropertyName = u.Property.Name,
                PropertyAddress = u.Property.Location == null
    ? "Address not set"
    : $"{u.Property.Location.BuildingNumber} {u.Property.Location.Street}, {u.Property.Location.City}"
            })
            .ToListAsync();

        return Ok(units);
    }

    // GET api/units/{id} — public, single unit details
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUnit(int id)
    {
        var unit = await _db.Units
            .Include(u => u.Property)
                .ThenInclude(p => p.Location)  // NEW
            .Where(u => u.UnitId == id)
            .Select(u => new UnitDto
            {
                UnitId = u.UnitId,
                UnitNumber = u.UnitNumber,
                UnitType = u.UnitType,
                Sizesqm = u.Sizesqm,
                MonthlyRent = u.MonthlyRent,
                Amenities = u.Amenities,
                AvailabilityStatus = u.AvailabilityStatus,
                PropertyName = u.Property.Name,
                PropertyAddress = u.Property.Location == null
    ? "Address not set"
    : $"{u.Property.Location.BuildingNumber} {u.Property.Location.Street}, {u.Property.Location.City}"
            })
            .FirstOrDefaultAsync();

        if (unit == null) return NotFound(new { message = "Unit not found." });
        return Ok(unit);
    }
}