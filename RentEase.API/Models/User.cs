using RentEase.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("Users")]
public partial class User
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    [Required]
    public string PasswordHash { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Required]
    [StringLength(20)]
    public string Role { get; set; } = "Tenant";

    public bool IsActive { get; set; } = true;

    [StringLength(20)]
    public string? AvailabilityStatus { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<MaintenanceRequest> MaintenanceRequestsAsTenant { get; set; } = new List<MaintenanceRequest>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}