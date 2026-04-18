using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentEase.API.Models;

public partial class Amenity
{
    [Key]
    [Column("AmenityID")]
    public int AmenityId { get; set; }

    [StringLength(100)]
    public string AmenityName { get; set; } = null!;

    [StringLength(250)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Icon { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Amenity")]
    public virtual ICollection<UnitAmenity> UnitAmenities { get; set; } = new List<UnitAmenity>();
}
