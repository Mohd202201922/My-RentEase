using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentEase.API.Models;

[Table("LeaseStatusHistory")]
[Index("LeaseId", Name = "IX_LeaseStatusHistory_LeaseID")]
[Index("StatusId", Name = "IX_LeaseStatusHistory_StatusID")]
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
    public DateTime ChangedAt { get; set; }

    public bool IsCurrent { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EffectiveDate { get; set; }

    [ForeignKey("ChangedByUserId")]
    [InverseProperty("LeaseStatusHistories")]
    public virtual User? ChangedByUser { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("LeaseStatusHistories")]
    public virtual Lease Lease { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("LeaseStatusHistories")]
    public virtual LeaseStatus Status { get; set; } = null!;
}
