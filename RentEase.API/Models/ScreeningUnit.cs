using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("ScreeningUnit")]
public partial class ScreeningUnit
{
    [Key]
    [Column("ScreeningUnitId")]
    public Guid ScreeningUnitId { get; set; } = Guid.NewGuid();

    [Column("LeaseApplicationId")]
    public Guid LeaseApplicationId { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime ScreeningAppointmentTime { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("LeaseApplicationId")]
    public virtual LeaseApplication LeaseApplication { get; set; } = null!;
}