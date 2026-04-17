using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("MaintenanceRequests")]
public partial class MaintenanceRequest
{
    [Key]
    [Column("RequestId")]
    public Guid RequestId { get; set; } = Guid.NewGuid();

    [Column("UnitId")]
    public Guid UnitId { get; set; }

    [Column("TenantId")]
    public Guid TenantId { get; set; }

    [Column("AssignedTo")]
    public Guid? AssignedTo { get; set; }

    [Required]
    [StringLength(50)]
    public string Category { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Required]
    [StringLength(1000)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("UnitId")]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("TenantId")]
    public virtual User Tenant { get; set; } = null!;

    [ForeignKey("AssignedTo")]
    public virtual User? AssignedStaffUser { get; set; }
}