using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentEase.API.Models;

[Table("MaintenanceRequest")]
[Index("StatusId", Name = "IX_MaintenanceRequest_StatusID")]
[Index("TicketNumber", Name = "UQ__Maintena__CBED06DA6A7B92C9", IsUnique = true)]
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

    [StringLength(100)]
    public string Title { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? RequestType { get; set; }

    [StringLength(50)]
    public string Priority { get; set; } = null!;

    [Column("StatusID")]
    public int? StatusId { get; set; }

    [StringLength(20)]
    public string? TicketNumber { get; set; }

    [Column("ChangedByUserID")]
    public int? ChangedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SubmittedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ResolvedAt { get; set; }

    [StringLength(500)]
    public string? ResolutionNotes { get; set; }

    [NotMapped]
    public string StatusName => Status?.StatusName ?? "Unknown";

    [ForeignKey("AssignedStaffId")]
    [InverseProperty("MaintenanceRequestAssignedStaffs")]
    public virtual User? AssignedStaff { get; set; }

    [InverseProperty("Request")]
    public virtual ICollection<MaintenanceStatusHistory> MaintenanceStatusHistories { get; set; } = new List<MaintenanceStatusHistory>();

    [ForeignKey("StatusId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual MaintenanceRequestStatus? Status { get; set; }

    [ForeignKey("TenantUserId")]
    [InverseProperty("MaintenanceRequestTenantUsers")]
    public virtual User TenantUser { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("MaintenanceRequests")]
    public virtual Unit Unit { get; set; } = null!;

    public virtual User? Tenant => TenantUser;
}
