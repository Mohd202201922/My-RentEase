using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentEase.API.Models;

[Index("AmenityId", Name = "IX_UnitAmenities_AmenityID")]
[Index("UnitId", Name = "IX_UnitAmenities_UnitID")]
public partial class UnitAmenity
{
    [Key]
    [Column("UnitAmenityID")]
    public int UnitAmenityId { get; set; }

    [Column("UnitID")]
    public int UnitId { get; set; }

    [Column("AmenityID")]
    public int AmenityId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("AmenityId")]
    [InverseProperty("UnitAmenities")]
    public virtual Amenity Amenity { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("UnitAmenities")]
    public virtual Unit Unit { get; set; } = null!;
}
