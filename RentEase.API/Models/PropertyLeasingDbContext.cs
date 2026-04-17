using Microsoft.EntityFrameworkCore;

namespace PropertyLeasing.API.Models;

public partial class PropertyLeasingDbContext : DbContext
{
    public PropertyLeasingDbContext() { }

    public PropertyLeasingDbContext(DbContextOptions<PropertyLeasingDbContext> options)
        : base(options) { }

    public virtual DbSet<Property> Properties { get; set; }
    public virtual DbSet<Unit> Units { get; set; }
    public virtual DbSet<LeaseApplication> LeaseApplications { get; set; }
    public virtual DbSet<Lease> Leases { get; set; }
    public virtual DbSet<PaymentRecord> PaymentRecords { get; set; }
    public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
    public virtual DbSet<MaintenanceRequestStatus> MaintenanceRequestStatuses { get; set; }
    public virtual DbSet<MaintenanceStatusHistory> MaintenanceStatusHistories { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }
    public virtual DbSet<Log> Logs { get; set; }
    public virtual DbSet<Document> Documents { get; set; }
    public virtual DbSet<Amenity> Amenities { get; set; }
    public virtual DbSet<UnitAmenity> UnitAmenities { get; set; }
    public virtual DbSet<LeaseApplicationStatus> LeaseApplicationStatuses { get; set; }
    public virtual DbSet<LeaseApplicationStatusHistory> LeaseApplicationStatusHistories { get; set; }
    public virtual DbSet<LeaseStatus> LeaseStatuses { get; set; }
    public virtual DbSet<LeaseStatusHistory> LeaseStatusHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Property -> Units
        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasOne(d => d.Property)
                .WithMany(p => p.Units)
                .HasConstraintName("FK_Unit_Property");
        });

        // Unit -> LeaseApplications
        modelBuilder.Entity<LeaseApplication>(entity =>
        {
            entity.HasOne(d => d.Unit)
                .WithMany(p => p.LeaseApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaseApplication_Unit");

            entity.HasOne(d => d.User)
                .WithMany(p => p.LeaseApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaseApplication_User");
        });

        // LeaseApplication -> Lease
        modelBuilder.Entity<Lease>(entity =>
        {
            entity.HasOne(d => d.Application)
                .WithMany(p => p.Leases)
                .HasConstraintName("FK_Lease_LeaseApplication");

            entity.HasOne(d => d.ParentLease)
                .WithMany(p => p.ChildLeases)
                .HasConstraintName("FK_Lease_ParentLease");
        });

        // Lease -> PaymentRecords
        modelBuilder.Entity<PaymentRecord>(entity =>
        {
            entity.HasOne(d => d.Lease)
                .WithMany(p => p.PaymentRecords)
                .HasConstraintName("FK_PaymentRecord_Lease");
        });

        // Unit -> MaintenanceRequests
        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.HasOne(d => d.Unit)
                .WithMany(p => p.MaintenanceRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceRequest_Unit");

            entity.HasOne(d => d.Tenant)
                .WithMany(p => p.MaintenanceRequestsAsTenant)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceRequest_Tenant");

            entity.HasOne(d => d.AssignedStaff)
                .WithMany(p => p.MaintenanceRequestsAsStaff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceRequest_Staff");

            entity.HasOne(d => d.Status)
                .WithMany(p => p.MaintenanceRequests)
                .HasConstraintName("FK_MaintenanceRequest_Status");
        });

        // MaintenanceRequest -> StatusHistory
        modelBuilder.Entity<MaintenanceStatusHistory>(entity =>
        {
            entity.HasOne(d => d.MaintenanceRequest)
                .WithMany(p => p.StatusHistory)
                .HasConstraintName("FK_StatusHistory_MaintenanceRequest");
        });

        // User -> Notifications
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasOne(d => d.User)
                .WithMany(p => p.Notifications)
                .HasConstraintName("FK_Notification_User");
        });

        // Unit -> Feedbacks
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasOne(d => d.Unit)
                .WithMany(p => p.Feedbacks)
                .HasConstraintName("FK_Feedback_Unit");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Feedbacks)
                .HasConstraintName("FK_Feedback_User");
        });

        // User -> Logs
        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasOne(d => d.User)
                .WithMany(p => p.Logs)
                .HasConstraintName("FK_Log_User");
        });

        // Document -> Application
        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasOne(d => d.Application)
                .WithMany(p => p.Documents)
                .HasConstraintName("FK_Document_Application");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Documents)
                .HasConstraintName("FK_Document_User");
        });

        modelBuilder.Entity<UnitAmenity>(entity =>
        {
            entity.HasOne(d => d.Unit)
                .WithMany(p => p.UnitAmenities)
                .HasConstraintName("FK_UnitAmenities_Unit");

            entity.HasOne(d => d.Amenity)
                .WithMany(p => p.UnitAmenities)
                .HasConstraintName("FK_UnitAmenities_Amenities");
        });

        modelBuilder.Entity<LeaseApplicationStatusHistory>(entity =>
        {
            entity.HasOne(d => d.Application)
                .WithMany(p => p.StatusHistory)
                .HasConstraintName("FK_AppStatusHistory_LeaseApplication");

            entity.HasOne(d => d.Status)
                .WithMany(p => p.StatusHistory)
                .HasConstraintName("FK_AppStatusHistory_Status");

            entity.HasOne(d => d.ChangedByUser)
                .WithMany()
                .HasConstraintName("FK_AppStatusHistory_User");
        });

        modelBuilder.Entity<LeaseStatusHistory>(entity =>
        {
            entity.HasOne(d => d.Lease)
                .WithMany(p => p.StatusHistory)
                .HasConstraintName("FK_LeaseStatusHistory_Lease");

            entity.HasOne(d => d.Status)
                .WithMany(p => p.StatusHistory)
                .HasConstraintName("FK_LeaseStatusHistory_Status");

            entity.HasOne(d => d.ChangedByUser)
                .WithMany()
                .HasConstraintName("FK_LeaseStatusHistory_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
