using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("MaintenanceRequest")]
public partial class MaintenanceRequest
{
    [Key]
    [Column("RequestID")]
    public int RequestId { get; set; }

    [Column("UnitID")]
    public int UnitId { get; set; }

    [Column("TenantUserID")]
    public int TenantUserId { get; set; }

    [Column("AssignedStaffID")]
    public int? AssignedStaffId { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    // Plumbing / Electrical / HVAC / General
    [StringLength(50)]
    public string? RequestType { get; set; }

    // Low / Medium / High / Urgent
    [StringLength(50)]
    public string Priority { get; set; } = "Medium";

    [Column("StatusID")]
    public int? StatusId { get; set; }

    // Unique ticket number for public lookup
    [StringLength(20)]
    public string? TicketNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SubmittedAt { get; set; } = DateTime.Now;

    [Column(TypeName = "datetime")]
    public DateTime? ResolvedAt { get; set; }

    [StringLength(500)]
    public string? ResolutionNotes { get; set; }

    [ForeignKey("UnitId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("TenantUserId")]
    [InverseProperty("MaintenanceRequestsAsTenant")]
    public virtual User Tenant { get; set; } = null!;

    [ForeignKey("AssignedStaffId")]
    [InverseProperty("MaintenanceRequestsAsStaff")]
    public virtual User? AssignedStaff { get; set; }

    [InverseProperty("MaintenanceRequest")]
    public virtual ICollection<MaintenanceStatusHistory> StatusHistory { get; set; } = new List<MaintenanceStatusHistory>();

    [ForeignKey("StatusId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual MaintenanceRequestStatus? Status { get; set; }
}
