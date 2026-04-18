using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentEase.API.Models;

[Table("Unit")]
public partial class Unit
{
    [Key]
    [Column("UnitID")]
    public int UnitId { get; set; }

    [Column("PropertyID")]
    public int PropertyId { get; set; }

    [StringLength(50)]
    public string UnitNumber { get; set; } = null!;

    [StringLength(50)]
    public string? UnitType { get; set; }

    public double? Sizesqm { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? MonthlyRent { get; set; }

    [StringLength(50)]
    public string? AvailabilityStatus { get; set; }

    [StringLength(100)]
    public string? ImgPath { get; set; }

    [NotMapped]
    public string? Amenities
    {
        get
        {
            return string.Join(", ", UnitAmenities?.Select(ua => ua.Amenity?.AmenityName ?? "") ?? Array.Empty<string>());
        }
    }

    [InverseProperty("Unit")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("Unit")]
    public virtual ICollection<LeaseApplication> LeaseApplications { get; set; } = new List<LeaseApplication>();

    [InverseProperty("Unit")]
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    [ForeignKey("PropertyId")]
    [InverseProperty("Units")]
    public virtual Property Property { get; set; } = null!;

    [InverseProperty("Unit")]
    public virtual ICollection<UnitAmenity> UnitAmenities { get; set; } = new List<UnitAmenity>();

    [InverseProperty("Unit")]
    public virtual ICollection<ScreeningAppointment> ScreeningAppointments { get; set; } = new List<ScreeningAppointment>();

    [InverseProperty("Unit")]
    public virtual ICollection<LeaseAgreement> LeaseAgreements { get; set; } = new List<LeaseAgreement>();
}