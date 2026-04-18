using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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

    [Column(TypeName = "datetime")]
    public DateTime? RequestedStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RequestedEndDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Application")]
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [InverseProperty("Application")]
    public virtual ICollection<LeaseApplicationStatusHistory> LeaseApplicationStatusHistories { get; set; } = new List<LeaseApplicationStatusHistory>();

    [InverseProperty("Application")]
    public virtual ICollection<Lease> Leases { get; set; } = new List<Lease>();

    [ForeignKey("UnitId")]
    [InverseProperty("LeaseApplications")]
    public virtual Unit Unit { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("LeaseApplications")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("Application")]
    public virtual ICollection<ScreeningAppointment> ScreeningAppointments { get; set; } = new List<ScreeningAppointment>();

    [InverseProperty("Application")]
    public virtual ICollection<LeaseAgreement> LeaseAgreements { get; set; } = new List<LeaseAgreement>();
}