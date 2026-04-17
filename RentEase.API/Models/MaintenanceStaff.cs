using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("MaintenanceStaff")]
public partial class MaintenanceStaff
{
    [Key]
    [Column("StaffId")]
    public Guid StaffId { get; set; } = Guid.NewGuid();

    [Column("UserId")]
    public Guid UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string Category { get; set; } = null!;

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}