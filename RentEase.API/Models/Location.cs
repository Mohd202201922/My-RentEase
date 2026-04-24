using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("Location")]
public class Location
{
    [Key]
    [Column("LocationID")]
    public int LocationId { get; set; }

    // ✅ No PropertyId column here – the relationship is purely via Property.LocationId

    [StringLength(20)]
    public string? BuildingNumber { get; set; }

    [StringLength(50)]
    public string? Block { get; set; }

    [StringLength(100)]
    public string? Street { get; set; }

    [Required]
    [StringLength(50)]
    public string City { get; set; } = null!;

    [StringLength(500)]
    public string? LocationUrl { get; set; }

    // Navigation back to Property (no foreign key attribute)
    public virtual Property Property { get; set; } = null!;
}