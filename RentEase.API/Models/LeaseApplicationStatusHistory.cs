using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RentEase.API.Models;

[Table("LeaseApplicationStatusHistory")]
[Index("ApplicationId", Name = "IX_LeaseAppStatusHistory_ApplicationID")]
[Index("StatusId", Name = "IX_LeaseAppStatusHistory_StatusID")]
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
    public DateTime ChangedAt { get; set; }

    public bool IsCurrent { get; set; }

    [ForeignKey("ApplicationId")]
    [InverseProperty("LeaseApplicationStatusHistories")]
    public virtual LeaseApplication Application { get; set; } = null!;

    [ForeignKey("ChangedByUserId")]
    [InverseProperty("LeaseApplicationStatusHistories")]
    public virtual User? ChangedByUser { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("LeaseApplicationStatusHistories")]
    public virtual LeaseApplicationStatus Status { get; set; } = null!;
}
