using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.DTOs;
using RentEase.API.Models;

namespace PropertyLeasing.API.Controllers;

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
            .Include(u => u.UnitAmenities)
                .ThenInclude(ua => ua.Amenity)
            .Where(u => u.AvailabilityStatus == "Available")
            .ToListAsync();

        return Ok(units.Select(u => new UnitDto
        {
            UnitId = u.UnitId,
            UnitNumber = u.UnitNumber,
            UnitType = u.UnitType,
            Sizesqm = u.Sizesqm,
            MonthlyRent = u.MonthlyRent,
            Amenities = string.Join(", ", u.UnitAmenities
                .Where(ua => ua.IsActive && ua.Amenity.IsActive)
                .OrderBy(ua => ua.Amenity.AmenityName)
                .Select(ua => ua.Amenity.AmenityName)),
            AvailabilityStatus = u.AvailabilityStatus,
            PropertyName = u.Property.Name,
            PropertyAddress = u.Property.Address
        }));
    }

    // GET api/units/{id} — public, single unit details
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUnit(int id)
    {
        var unit = await _db.Units
            .Include(u => u.Property)
            .Include(u => u.UnitAmenities)
                .ThenInclude(ua => ua.Amenity)
            .Where(u => u.UnitId == id)
            .FirstOrDefaultAsync();

        if (unit == null) return NotFound(new { message = "Unit not found." });
        return Ok(new UnitDto
        {
            UnitId = unit.UnitId,
            UnitNumber = unit.UnitNumber,
            UnitType = unit.UnitType,
            Sizesqm = unit.Sizesqm,
            MonthlyRent = unit.MonthlyRent,
            Amenities = string.Join(", ", unit.UnitAmenities
                .Where(ua => ua.IsActive && ua.Amenity.IsActive)
                .OrderBy(ua => ua.Amenity.AmenityName)
                .Select(ua => ua.Amenity.AmenityName)),
            AvailabilityStatus = unit.AvailabilityStatus,
            PropertyName = unit.Property.Name,
            PropertyAddress = unit.Property.Address
        });
    }
}
