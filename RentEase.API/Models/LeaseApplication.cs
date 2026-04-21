using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("LeaseApplication")]
public partial class LeaseApplication
{
    [Key]
    [Column("ApplicationID")]
    public int ApplicationId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("UnitID")]
    public int UnitId { get; set; }

    public DateTime? RequestedStartDate { get; set; }
    public DateTime? RequestedEndDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }  // Pending, Approved, Rejected, Renewal, Terminated

    public DateTime? UpdatedAt { get; set; }

    // Payment fields
    public bool IsPaymentApproved { get; set; }
    public DateTime? PaymentApprovedAt { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? PaymentTransactionId { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal? PaymentAmount { get; set; }

    // Termination request
    public bool TerminationRequested { get; set; }
    public DateTime? TerminationRequestDate { get; set; }
    public DateTime? TerminationApprovedAt { get; set; }
    public DateTime? TerminationMoveOutDate { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
    public virtual ICollection<LeaseApplicationStatusHistory> LeaseApplicationStatusHistories { get; set; } = new List<LeaseApplicationStatusHistory>();
    public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();
    public virtual Unit Unit { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}