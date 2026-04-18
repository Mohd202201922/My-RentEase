using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentEase.API.Models;

[Table("PaymentRecord")]
public partial class PaymentRecord
{
    [Key]
    [Column("PaymentID")]
    public int PaymentId { get; set; }

    [Column("LeaseID")]
    public int LeaseId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal AmountDue { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? AmountPaid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DueDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PaidDate { get; set; }

    [StringLength(50)]
    public string PaymentStatus { get; set; } = null!;

    [StringLength(250)]
    public string? Notes { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("PaymentRecords")]
    public virtual Lease Lease { get; set; } = null!;
}
