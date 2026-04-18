using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("Notification")]
public partial class Notification
{
    [Key]
    [Column("NotificationID")]
    public int NotificationId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Required]
    [StringLength(500)]
    public string Message { get; set; } = null!;

    [StringLength(50)]
    public string? NotificationType { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = "Unread";

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual User User { get; set; } = null!;
}

[Table("Feedback")]
public partial class Feedback
{
    [Key]
    [Column("FeedbackID")]
    public int FeedbackId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("UnitID")]
    public int UnitId { get; set; }

    public int? Rating { get; set; }

    [StringLength(500)]
    public string? Comment { get; set; }

    public bool IsVisible { get; set; } = true;

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("UserId")]
    [InverseProperty("Feedbacks")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("Feedbacks")]
    public virtual Unit Unit { get; set; } = null!;
}

[Table("Log")]
public partial class Log
{
    [Key]
    [Column("LogID")]
    public int LogId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [StringLength(100)]
    public string? Action { get; set; }

    [StringLength(500)]
    public string? Details { get; set; }

    [StringLength(20)]
    public string? LogLevel { get; set; }

    [StringLength(20)]
    public string? Source { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("UserId")]
    [InverseProperty("Logs")]
    public virtual User? User { get; set; }
}

[Table("Document")]
public partial class Document
{
    [Key]
    [Column("DocumentID")]
    public int DocumentId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [Column("ApplicationID")]
    public int? ApplicationId { get; set; }

    [Required]
    [StringLength(200)]
    public string FileName { get; set; } = null!;

    [StringLength(50)]
    public string? FileType { get; set; }

    [StringLength(500)]
    public string? StoragePath { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime UploadedAt { get; set; } = DateTime.Now;

    [ForeignKey("UserId")]
    [InverseProperty("Documents")]
    public virtual User? User { get; set; }

    [ForeignKey("ApplicationId")]
    [InverseProperty("Documents")]
    public virtual LeaseApplication? Application { get; set; }
}

// ============ SCREENING APPOINTMENT MODEL ============
[Table("ScreeningAppointment")]
public partial class ScreeningAppointment
{
    [Key]
    [Column("ScreeningId")]
    public int ScreeningId { get; set; }

    [Column("ApplicationId")]
    public int ApplicationId { get; set; }

    [Column("UnitId")]
    public int UnitId { get; set; }

    [Column("TenantId")]
    public int TenantId { get; set; }

    [Column("ScheduledDate")]
    public DateTime ScheduledDate { get; set; }

    [Column("EndTime")]
    public DateTime EndTime { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = "Pending";

    [StringLength(500)]
    public string? Notes { get; set; }

    [Column("ManagerNotes")]
    [StringLength(500)]
    public string? ManagerNotes { get; set; }

    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("UpdatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ApplicationId")]
    [InverseProperty("ScreeningAppointments")]
    public virtual LeaseApplication Application { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("ScreeningAppointments")]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("TenantId")]
    [InverseProperty("ScreeningAppointments")]
    public virtual User Tenant { get; set; } = null!;

    // Navigation to LeaseAgreement (one-to-one)
    public virtual LeaseAgreement? LeaseAgreement { get; set; }
}

// ============ LEASE AGREEMENT MODEL ============
[Table("LeaseAgreement")]
public partial class LeaseAgreement
{
    [Key]
    [Column("LeaseAgreementId")]
    public int LeaseAgreementId { get; set; }

    [Column("ApplicationId")]
    public int ApplicationId { get; set; }

    [Column("ScreeningId")]
    public int ScreeningId { get; set; }

    [Column("UnitId")]
    public int UnitId { get; set; }

    [Column("TenantId")]
    public int TenantId { get; set; }

    [Column("LeaseStartDate")]
    public DateTime LeaseStartDate { get; set; }

    [Column("LeaseEndDate")]
    public DateTime LeaseEndDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal MonthlyRent { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal SecurityDeposit { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? LateFeePerDay { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = "Draft";

    [StringLength(500)]
    public string? TermsAndConditions { get; set; }

    [StringLength(500)]
    public string? SpecialClauses { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SignedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ApplicationId")]
    [InverseProperty("LeaseAgreements")]
    public virtual LeaseApplication Application { get; set; } = null!;

    [ForeignKey("ScreeningId")]
    [InverseProperty("LeaseAgreement")]
    public virtual ScreeningAppointment Screening { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("LeaseAgreements")]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("TenantId")]
    [InverseProperty("LeaseAgreements")]
    public virtual User Tenant { get; set; } = null!;
}