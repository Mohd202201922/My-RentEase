using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentEase.API.Models;

[Table("User")]
[Index("Email", Name = "UQ__User__A9D105342EFAB69E", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(50)]
    public string Role { get; set; } = null!;

    [StringLength(200)]
    public string? SkillProfile { get; set; }

    [StringLength(50)]
    public string? AvailabilityStatus { get; set; }

    [StringLength(450)]
    public string? IdentityUserId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [InverseProperty("User")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("ChangedByUser")]
    public virtual ICollection<LeaseApplicationStatusHistory> LeaseApplicationStatusHistories { get; set; } = new List<LeaseApplicationStatusHistory>();

    [InverseProperty("User")]
    public virtual ICollection<LeaseApplication> LeaseApplications { get; set; } = new List<LeaseApplication>();

    [InverseProperty("ChangedByUser")]
    public virtual ICollection<LeaseStatusHistory> LeaseStatusHistories { get; set; } = new List<LeaseStatusHistory>();

    [InverseProperty("User")]
    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    [InverseProperty("AssignedStaff")]
    public virtual ICollection<MaintenanceRequest> MaintenanceRequestAssignedStaffs { get; set; } = new List<MaintenanceRequest>();

    [InverseProperty("TenantUser")]
    public virtual ICollection<MaintenanceRequest> MaintenanceRequestTenantUsers { get; set; } = new List<MaintenanceRequest>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    
}