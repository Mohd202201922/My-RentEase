using PropertyLeasing.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("Units")]
public partial class Unit
{
    [Key]
    [Column("UnitId")]
    public Guid UnitId { get; set; } = Guid.NewGuid();

    [Column("PropertyId")]
    public Guid PropertyId { get; set; }

    [Required]
    [StringLength(50)]
    public string UnitNumber { get; set; } = null!;

    [Required]
    [StringLength(50)]
    public string Type { get; set; } = null!;          // matches SQL

    [Column(TypeName = "decimal(10,2)")]
    public decimal? Size { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RentAmount { get; set; }

    [StringLength(250)]
    public string? Amenities { get; set; }             // string column in SQL

    [Required]
    [StringLength(20)]
    public string AvailabilityStatus { get; set; } = null!;

    [StringLength(100)]
    public string? ImgPath { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("PropertyId")]
    public virtual Property Property { get; set; } = null!;

    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}