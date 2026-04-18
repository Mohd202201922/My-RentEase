using Microsoft.EntityFrameworkCore;

namespace RentEase.API.Models;

public partial class PropertyLeasingDbContext : DbContext
{
    public PropertyLeasingDbContext(DbContextOptions<PropertyLeasingDbContext> options)
        : base(options) { }

    public virtual DbSet<Property> Properties { get; set; }
    public virtual DbSet<Unit> Units { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<MaintenanceStaff> MaintenanceStaff { get; set; }
    public virtual DbSet<LeaseApplication> LeaseApplications { get; set; }
    public virtual DbSet<LeaseAgreement> LeaseAgreements { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
    public virtual DbSet<ScreeningUnit> ScreeningUnits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Property)
            .WithMany(p => p.Units)
            .HasForeignKey(u => u.PropertyId);

        modelBuilder.Entity<MaintenanceRequest>()
            .HasOne(mr => mr.Unit)
            .WithMany(u => u.MaintenanceRequests)
            .HasForeignKey(mr => mr.UnitId);

        modelBuilder.Entity<MaintenanceRequest>()
            .HasOne(mr => mr.Tenant)
            .WithMany()
            .HasForeignKey(mr => mr.TenantId);

        modelBuilder.Entity<MaintenanceRequest>()
            .HasOne(mr => mr.AssignedStaffUser)
            .WithMany()
            .HasForeignKey(mr => mr.AssignedTo)
            .OnDelete(DeleteBehavior.SetNull);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}