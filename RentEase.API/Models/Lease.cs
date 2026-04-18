using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentEase.API.Models;

[Table("Lease")]
[Index("ParentLeaseId", Name = "IX_Lease_ParentLeaseID")]
public partial class Lease
{
    [Key]
    [Column("LeaseID")]
    public int LeaseId { get; set; }

    [Column("ApplicationID")]
    public int ApplicationId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LeaseStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LeaseEndDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal MonthlyRent { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal SecurityDeposit { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("ParentLeaseID")]
    public int? ParentLeaseId { get; set; }

    [StringLength(500)]
    public string? TerminationReason { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TerminationDate { get; set; }

    [ForeignKey("ApplicationId")]
    [InverseProperty("Leases")]
    public virtual LeaseApplication Application { get; set; } = null!;

    [InverseProperty("ParentLease")]
    public virtual ICollection<Lease> InverseParentLease { get; set; } = new List<Lease>();

    [InverseProperty("Lease")]
    public virtual ICollection<LeaseStatusHistory> LeaseStatusHistories { get; set; } = new List<LeaseStatusHistory>();

    [ForeignKey("ParentLeaseId")]
    [InverseProperty("InverseParentLease")]
    public virtual Lease? ParentLease { get; set; }

    [InverseProperty("Lease")]
    public virtual ICollection<PaymentRecord> PaymentRecords { get; set; } = new List<PaymentRecord>();

    [NotMapped]
    public string Status => LeaseStatusHistories.FirstOrDefault(h => h.IsCurrent)?.Status?.StatusName ?? "Unknown";
}
