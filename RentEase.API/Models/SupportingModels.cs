using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyLeasing.API.Models;

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

    // LeaseUpdate / MaintenanceUpdate / PaymentReminder / General
    [StringLength(50)]
    public string? NotificationType { get; set; }

    // Read / Unread
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

    // Info / Warning / Error
    [StringLength(20)]
    public string? LogLevel { get; set; }

    // Web / API
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

[Table("Amenities")]
public partial class Amenity
{
    [Key]
    [Column("AmenityID")]
    public int AmenityId { get; set; }

    [Required]
    [Column("AmenityName")]
    [StringLength(100)]
    public string AmenityName { get; set; } = null!;

    [StringLength(250)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? Icon { get; set; }

    public bool IsActive { get; set; } = true;

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [InverseProperty("Amenity")]
    public virtual ICollection<UnitAmenity> UnitAmenities { get; set; } = new List<UnitAmenity>();
}

[Table("UnitAmenities")]
public partial class UnitAmenity
{
    [Key]
    [Column("UnitAmenityID")]
    public int UnitAmenityId { get; set; }

    [Column("UnitID")]
    public int UnitId { get; set; }

    [Column("AmenityID")]
    public int AmenityId { get; set; }

    public bool IsActive { get; set; } = true;

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey("AmenityId")]
    [InverseProperty("UnitAmenities")]
    public virtual Amenity Amenity { get; set; } = null!;

    [ForeignKey("UnitId")]
    [InverseProperty("UnitAmenities")]
    public virtual Unit Unit { get; set; } = null!;
}

[Table("MaintenanceRequestStatus")]
public partial class MaintenanceRequestStatus
{
    [Key]
    [Column("StatusID")]
    public int StatusId { get; set; }

    [Required]
    [Column("StatusName")]
    [StringLength(50)]
    public string StatusName { get; set; } = null!;

    [StringLength(250)]
    public string? Description { get; set; }

    public int? DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [InverseProperty("Status")]
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();
}

[Table("LeaseApplicationStatus")]
public partial class LeaseApplicationStatus
{
    [Key]
    [Column("StatusID")]
    public int StatusId { get; set; }

    [Required]
    [Column("StatusName")]
    [StringLength(50)]
    public string StatusName { get; set; } = null!;

    [StringLength(250)]
    public string? Description { get; set; }

    public int? DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsFinal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [InverseProperty("Status")]
    public virtual ICollection<LeaseApplicationStatusHistory> StatusHistory { get; set; } = new List<LeaseApplicationStatusHistory>();
}

[Table("LeaseApplicationStatusHistory")]
public partial class LeaseApplicationStatusHistory
{
    [Key]
    [Column("ApplicationStatusHistoryID")]
    public int ApplicationStatusHistoryId { get; set; }

    [Column("ApplicationID")]
    public int ApplicationId { get; set; }

    [Column("StatusID")]
    public int StatusId { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [Column("ChangedByUserID")]
    public int? ChangedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ChangedAt { get; set; } = DateTime.Now;

    public bool IsCurrent { get; set; }

    [ForeignKey("ApplicationId")]
    [InverseProperty("StatusHistory")]
    public virtual LeaseApplication Application { get; set; } = null!;

    [ForeignKey("ChangedByUserId")]
    public virtual User? ChangedByUser { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("StatusHistory")]
    public virtual LeaseApplicationStatus Status { get; set; } = null!;
}

[Table("LeaseStatus")]
public partial class LeaseStatus
{
    [Key]
    [Column("StatusID")]
    public int StatusId { get; set; }

    [Required]
    [Column("StatusName")]
    [StringLength(50)]
    public string StatusName { get; set; } = null!;

    [StringLength(250)]
    public string? Description { get; set; }

    public int? DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsTerminal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [InverseProperty("Status")]
    public virtual ICollection<LeaseStatusHistory> StatusHistory { get; set; } = new List<LeaseStatusHistory>();
}

[Table("LeaseStatusHistory")]
public partial class LeaseStatusHistory
{
    [Key]
    [Column("LeaseStatusHistoryID")]
    public int LeaseStatusHistoryId { get; set; }

    [Column("LeaseID")]
    public int LeaseId { get; set; }

    [Column("StatusID")]
    public int StatusId { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [Column("ChangedByUserID")]
    public int? ChangedByUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ChangedAt { get; set; } = DateTime.Now;

    public bool IsCurrent { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EffectiveDate { get; set; } = DateTime.Now;

    [ForeignKey("ChangedByUserId")]
    public virtual User? ChangedByUser { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("StatusHistory")]
    public virtual Lease Lease { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("StatusHistory")]
    public virtual LeaseStatus Status { get; set; } = null!;
}
