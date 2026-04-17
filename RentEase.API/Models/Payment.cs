using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("Payments")]
public partial class Payment
{
    [Key]
    [Column("PaymentId")]
    public Guid PaymentId { get; set; } = Guid.NewGuid();

    [Column("LeaseId")]
    public Guid LeaseId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountDue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? AmountPaid { get; set; }

    [Column(TypeName = "date")]
    public DateTime DueDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? PaidDate { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(50)]
    public string? PaymentMethod { get; set; }

    [StringLength(250)]
    public string? Notes { get; set; }

    [Column("RecordedBy")]
    public Guid RecordedBy { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("LeaseId")]
    public virtual LeaseAgreement Lease { get; set; } = null!;
}