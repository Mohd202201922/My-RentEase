using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RentEase.API.Models;

namespace RentEase.API.Data;

public partial class PropertyLeasingDbContext : DbContext
{
    public PropertyLeasingDbContext(DbContextOptions<PropertyLeasingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Amenity> Amenities { get; set; }
    public virtual DbSet<Document> Documents { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }
    public virtual DbSet<Lease> Leases { get; set; }
    public virtual DbSet<LeaseApplication> LeaseApplications { get; set; }
    public virtual DbSet<LeaseApplicationStatus> LeaseApplicationStatuses { get; set; }
    public virtual DbSet<LeaseApplicationStatusHistory> LeaseApplicationStatusHistories { get; set; }
    public virtual DbSet<LeaseStatus> LeaseStatuses { get; set; }
    public virtual DbSet<LeaseStatusHistory> LeaseStatusHistories { get; set; }
    public virtual DbSet<Log> Logs { get; set; }
    public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
    public virtual DbSet<MaintenanceRequestStatus> MaintenanceRequestStatuses { get; set; }
    public virtual DbSet<MaintenanceStatusHistory> MaintenanceStatusHistories { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<PaymentRecord> PaymentRecords { get; set; }
    public virtual DbSet<Property> Properties { get; set; }
    public virtual DbSet<Unit> Units { get; set; }
    public virtual DbSet<UnitAmenity> UnitAmenities { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ========================================
        // Amenity
        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.AmenityId).HasName("PK__Amenitie__842AF52B66D7181C");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Document
        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__Document__1ABEEF6FDC70F0D5");
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");
            entity.HasOne(d => d.Application).WithMany(p => p.Documents).HasConstraintName("FK_Document_Application");
            entity.HasOne(d => d.User).WithMany(p => p.Documents).HasConstraintName("FK_Document_User");
        });

        // Feedback
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF61FA2CC73");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsVisible).HasDefaultValue(true);
            entity.HasOne(d => d.Unit).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedback_Unit");
            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedback_User");
        });

        // Lease
        modelBuilder.Entity<Lease>(entity =>
        {
            entity.HasKey(e => e.LeaseId).HasName("PK__Lease__21FA58E1C76373B6");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.HasOne(d => d.Application).WithMany(p => p.Leases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lease_LeaseApplication");
            entity.HasOne(d => d.ParentLease).WithMany(p => p.InverseParentLease).HasConstraintName("FK_Lease_ParentLease");
        });

        // LeaseApplication
        modelBuilder.Entity<LeaseApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__LeaseApp__C93A4F79BC1A5CB8");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsPaymentApproved).HasDefaultValue(false);
            entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10,2)");
            entity.Property(e => e.TerminationRequested).HasDefaultValue(false);

            entity.HasOne(d => d.Unit).WithMany(p => p.LeaseApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaseApplication_Unit");
            entity.HasOne(d => d.User).WithMany(p => p.LeaseApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaseApplication_User");
        });

        // LeaseApplicationStatus
        modelBuilder.Entity<LeaseApplicationStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__LeaseApp__C8EE20437A0C0CE2");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // LeaseApplicationStatusHistory
        modelBuilder.Entity<LeaseApplicationStatusHistory>(entity =>
        {
            entity.HasKey(e => e.ApplicationStatusHistoryId).HasName("PK__LeaseApp__828D91DEE4081288");
            entity.HasIndex(e => e.IsCurrent, "IX_LeaseAppStatusHistory_IsCurrent").HasFilter("([IsCurrent]=(1))");
            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(getdate())");
            entity.HasOne(d => d.Application).WithMany(p => p.LeaseApplicationStatusHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppStatusHistory_LeaseApplication");
            entity.HasOne(d => d.ChangedByUser).WithMany(p => p.LeaseApplicationStatusHistories)
                .HasConstraintName("FK_AppStatusHistory_User");
            entity.HasOne(d => d.Status).WithMany(p => p.LeaseApplicationStatusHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppStatusHistory_Status");
        });

        // LeaseStatus
        modelBuilder.Entity<LeaseStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__LeaseSta__C8EE2043F72AD14C");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // LeaseStatusHistory
        modelBuilder.Entity<LeaseStatusHistory>(entity =>
        {
            entity.HasKey(e => e.LeaseStatusHistoryId).HasName("PK__LeaseSta__5DF4886218D2F8BF");
            entity.HasIndex(e => e.IsCurrent, "IX_LeaseStatusHistory_IsCurrent").HasFilter("([IsCurrent]=(1))");
            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EffectiveDate).HasDefaultValueSql("(getdate())");
            entity.HasOne(d => d.ChangedByUser).WithMany(p => p.LeaseStatusHistories)
                .HasConstraintName("FK_LeaseStatusHistory_User");
            entity.HasOne(d => d.Lease).WithMany(p => p.LeaseStatusHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaseStatusHistory_Lease");
            entity.HasOne(d => d.Status).WithMany(p => p.LeaseStatusHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaseStatusHistory_Status");
        });

        // Log
        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Log__5E5499A88C9281C8");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.HasOne(d => d.User).WithMany(p => p.Logs).HasConstraintName("FK_Log_User");
        });

        // MaintenanceRequest
        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Maintena__33A8519AC78BB497");
            entity.Property(e => e.Priority).HasDefaultValue("Medium");
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(getdate())");
            entity.HasOne(d => d.AssignedStaff).WithMany(p => p.MaintenanceRequestAssignedStaffs)
                .HasConstraintName("FK_MaintenanceRequest_Staff");
            entity.HasOne(d => d.Status).WithMany(p => p.MaintenanceRequests)
                .HasConstraintName("FK_MaintenanceRequest_Status");
            entity.HasOne(d => d.TenantUser).WithMany(p => p.MaintenanceRequestTenantUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceRequest_Tenant");
            entity.HasOne(d => d.Unit).WithMany(p => p.MaintenanceRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceRequest_Unit");
        });

        // MaintenanceRequestStatus
        modelBuilder.Entity<MaintenanceRequestStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Maintena__C8EE204319ED2289");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // MaintenanceStatusHistory
        modelBuilder.Entity<MaintenanceStatusHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__Maintena__4D7B4ADD26D66E04");
            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(getdate())");
            entity.HasOne(d => d.Request).WithMany(p => p.MaintenanceStatusHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusHistory_MaintenanceRequest");
        });

        // Notification
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32E20FD78C");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Unread");
            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_User");
        });

        // PaymentRecord
        modelBuilder.Entity<PaymentRecord>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__PaymentR__9B556A5899B11BFD");
            entity.Property(e => e.PaymentStatus).HasDefaultValue("Pending");
            entity.HasOne(d => d.Lease).WithMany(p => p.PaymentRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentRecord_Lease");
        });

        // Property
        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.PropertyId).HasName("PK__Property__70C9A75539279CAD");
        });

        // Unit
        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__Unit__44F5EC9566254066");
            entity.Property(e => e.AvailabilityStatus).HasDefaultValue("Available");
            entity.HasOne(d => d.Property).WithMany(p => p.Units)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Unit_Property");
        });

        // UnitAmenity
        modelBuilder.Entity<UnitAmenity>(entity =>
        {
            entity.HasKey(e => e.UnitAmenityId).HasName("PK__UnitAmen__3F6BBFB16E562622");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasOne(d => d.Amenity).WithMany(p => p.UnitAmenities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitAmenities_Amenities");
            entity.HasOne(d => d.Unit).WithMany(p => p.UnitAmenities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitAmenities_Unit");
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACEAA9FFB0");
            entity.Property(e => e.Role).HasDefaultValue("Tenant");
        });

        // ========================================
        // Seed Data (simplified for brevity – keep your existing seed data)
        // ========================================
        // (I am omitting the full SeedData method to save space; keep your existing one)
        // Make sure you remove any references to ScreeningAppointment or LeaseAgreement from SeedData.

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}