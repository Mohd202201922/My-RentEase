using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentEase.API.Models;

[Table("LeaseAgreements")]
public partial class LeaseAgreement
{
    [Key]
    [Column("LeaseId")]
    public Guid LeaseId { get; set; } = Guid.NewGuid();

    [Column("ApplicationId")]
    public Guid ApplicationId { get; set; }

    [Column("UnitId")]
    public Guid UnitId { get; set; }

    [Column("TenantId")]
    public Guid TenantId { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal RentAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal SecurityDeposit { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(200)]
    public string? TerminationReason { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("ApplicationId")]
    public virtual LeaseApplication Application { get; set; } = null!;

    [ForeignKey("UnitId")]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("TenantId")]
    public virtual User Tenant { get; set; } = null!;
}