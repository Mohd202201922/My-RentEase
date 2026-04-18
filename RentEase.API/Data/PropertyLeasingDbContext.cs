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

    // NEW DbSets
    public virtual DbSet<ScreeningAppointment> ScreeningAppointments { get; set; }
    public virtual DbSet<LeaseAgreement> LeaseAgreements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ========================================
        // EXISTING CONFIGURATIONS
        // ========================================

        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.AmenityId).HasName("PK__Amenitie__842AF52B66D7181C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__Document__1ABEEF6FDC70F0D5");

            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Application).WithMany(p => p.Documents).HasConstraintName("FK_Document_Application");

            entity.HasOne(d => d.User).WithMany(p => p.Documents).HasConstraintName("FK_Document_User");
        });

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

        modelBuilder.Entity<Lease>(entity =>
        {
            entity.HasKey(e => e.LeaseId).HasName("PK__Lease__21FA58E1C76373B6");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Application).WithMany(p => p.Leases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lease_LeaseApplication");

            entity.HasOne(d => d.ParentLease).WithMany(p => p.InverseParentLease).HasConstraintName("FK_Lease_ParentLease");
        });

        modelBuilder.Entity<LeaseApplication>(entity =>
        {
            entity.HasKey(e => e.ApplicationId).HasName("PK__LeaseApp__C93A4F79BC1A5CB8");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Unit).WithMany(p => p.LeaseApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaseApplication_Unit");

            entity.HasOne(d => d.User).WithMany(p => p.LeaseApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaseApplication_User");
        });

        modelBuilder.Entity<LeaseApplicationStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__LeaseApp__C8EE20437A0C0CE2");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<LeaseApplicationStatusHistory>(entity =>
        {
            entity.HasKey(e => e.ApplicationStatusHistoryId).HasName("PK__LeaseApp__828D91DEE4081288");

            entity.HasIndex(e => e.IsCurrent, "IX_LeaseAppStatusHistory_IsCurrent").HasFilter("([IsCurrent]=(1))");

            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Application).WithMany(p => p.LeaseApplicationStatusHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppStatusHistory_LeaseApplication");

            entity.HasOne(d => d.ChangedByUser).WithMany(p => p.LeaseApplicationStatusHistories).HasConstraintName("FK_AppStatusHistory_User");

            entity.HasOne(d => d.Status).WithMany(p => p.LeaseApplicationStatusHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppStatusHistory_Status");
        });

        modelBuilder.Entity<LeaseStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__LeaseSta__C8EE2043F72AD14C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<LeaseStatusHistory>(entity =>
        {
            entity.HasKey(e => e.LeaseStatusHistoryId).HasName("PK__LeaseSta__5DF4886218D2F8BF");

            entity.HasIndex(e => e.IsCurrent, "IX_LeaseStatusHistory_IsCurrent").HasFilter("([IsCurrent]=(1))");

            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EffectiveDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ChangedByUser).WithMany(p => p.LeaseStatusHistories).HasConstraintName("FK_LeaseStatusHistory_User");

            entity.HasOne(d => d.Lease).WithMany(p => p.LeaseStatusHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaseStatusHistory_Lease");

            entity.HasOne(d => d.Status).WithMany(p => p.LeaseStatusHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeaseStatusHistory_Status");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Log__5E5499A88C9281C8");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Logs).HasConstraintName("FK_Log_User");
        });

        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Maintena__33A8519AC78BB497");

            entity.Property(e => e.Priority).HasDefaultValue("Medium");
            entity.Property(e => e.SubmittedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.AssignedStaff).WithMany(p => p.MaintenanceRequestAssignedStaffs).HasConstraintName("FK_MaintenanceRequest_Staff");

            entity.HasOne(d => d.Status).WithMany(p => p.MaintenanceRequests).HasConstraintName("FK_MaintenanceRequest_Status");

            entity.HasOne(d => d.TenantUser).WithMany(p => p.MaintenanceRequestTenantUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceRequest_Tenant");

            entity.HasOne(d => d.Unit).WithMany(p => p.MaintenanceRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceRequest_Unit");
        });

        modelBuilder.Entity<MaintenanceRequestStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Maintena__C8EE204319ED2289");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<MaintenanceStatusHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__Maintena__4D7B4ADD26D66E04");

            entity.Property(e => e.ChangedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Request).WithMany(p => p.MaintenanceStatusHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusHistory_MaintenanceRequest");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32E20FD78C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Unread");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_User");
        });

        modelBuilder.Entity<PaymentRecord>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__PaymentR__9B556A5899B11BFD");

            entity.Property(e => e.PaymentStatus).HasDefaultValue("Pending");

            entity.HasOne(d => d.Lease).WithMany(p => p.PaymentRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentRecord_Lease");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.PropertyId).HasName("PK__Property__70C9A75539279CAD");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__Unit__44F5EC9566254066");

            entity.Property(e => e.AvailabilityStatus).HasDefaultValue("Available");

            entity.HasOne(d => d.Property).WithMany(p => p.Units)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Unit_Property");
        });

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

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACEAA9FFB0");

            entity.Property(e => e.Role).HasDefaultValue("Tenant");
        });

        // ========================================
        // NEW CONFIGURATIONS FOR SCREENING AND LEASE AGREEMENT
        // ========================================

        // Configure ScreeningAppointment
        modelBuilder.Entity<ScreeningAppointment>(entity =>
        {
            entity.HasKey(e => e.ScreeningId);

            entity.Property(e => e.Status).HasDefaultValue("Pending");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(e => e.Application)
                .WithMany(e => e.ScreeningAppointments)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Unit)
                .WithMany(e => e.ScreeningAppointments)
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Tenant)
                .WithMany(e => e.ScreeningAppointments)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure LeaseAgreement
        modelBuilder.Entity<LeaseAgreement>(entity =>
        {
            entity.HasKey(e => e.LeaseAgreementId);

            entity.Property(e => e.Status).HasDefaultValue("Draft");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(e => e.Application)
                .WithMany(e => e.LeaseAgreements)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Screening)
                .WithOne(e => e.LeaseAgreement)
                .HasForeignKey<LeaseAgreement>(e => e.ScreeningId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Unit)
                .WithMany(e => e.LeaseAgreements)
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Tenant)
                .WithMany(e => e.LeaseAgreements)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed Data
        SeedData(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // ========================================
        // 1. SEED STATUS TABLES FIRST
        // ========================================

        // Maintenance Request Statuses
        modelBuilder.Entity<MaintenanceRequestStatus>().HasData(
            new MaintenanceRequestStatus { StatusId = 1, StatusName = "Submitted", Description = "Request has been submitted", DisplayOrder = 1, IsActive = true, CreatedAt = DateTime.Now },
            new MaintenanceRequestStatus { StatusId = 2, StatusName = "Assigned", Description = "Assigned to staff", DisplayOrder = 2, IsActive = true, CreatedAt = DateTime.Now },
            new MaintenanceRequestStatus { StatusId = 3, StatusName = "InProgress", Description = "Work in progress", DisplayOrder = 3, IsActive = true, CreatedAt = DateTime.Now },
            new MaintenanceRequestStatus { StatusId = 4, StatusName = "Resolved", Description = "Issue resolved", DisplayOrder = 4, IsActive = true, CreatedAt = DateTime.Now },
            new MaintenanceRequestStatus { StatusId = 5, StatusName = "Closed", Description = "Request closed", DisplayOrder = 5, IsActive = true, CreatedAt = DateTime.Now }
        );

        // Lease Application Statuses
        modelBuilder.Entity<LeaseApplicationStatus>().HasData(
            new LeaseApplicationStatus { StatusId = 1, StatusName = "Pending", Description = "Awaiting review", DisplayOrder = 1, IsActive = true, IsFinal = false, CreatedAt = DateTime.Now },
            new LeaseApplicationStatus { StatusId = 2, StatusName = "Screening", Description = "Application under review", DisplayOrder = 2, IsActive = true, IsFinal = false, CreatedAt = DateTime.Now },
            new LeaseApplicationStatus { StatusId = 3, StatusName = "Approved", Description = "Application approved", DisplayOrder = 3, IsActive = true, IsFinal = false, CreatedAt = DateTime.Now },
            new LeaseApplicationStatus { StatusId = 4, StatusName = "Rejected", Description = "Application rejected", DisplayOrder = 4, IsActive = true, IsFinal = true, CreatedAt = DateTime.Now }
        );

        // Lease Statuses
        modelBuilder.Entity<LeaseStatus>().HasData(
            new LeaseStatus { StatusId = 1, StatusName = "Active", Description = "Lease is active", DisplayOrder = 1, IsActive = true, IsTerminal = false, CreatedAt = DateTime.Now },
            new LeaseStatus { StatusId = 2, StatusName = "Expired", Description = "Lease has expired", DisplayOrder = 2, IsActive = true, IsTerminal = true, CreatedAt = DateTime.Now },
            new LeaseStatus { StatusId = 3, StatusName = "Terminated", Description = "Lease terminated early", DisplayOrder = 3, IsActive = true, IsTerminal = true, CreatedAt = DateTime.Now },
            new LeaseStatus { StatusId = 4, StatusName = "Renewed", Description = "Lease has been renewed", DisplayOrder = 4, IsActive = true, IsTerminal = false, CreatedAt = DateTime.Now }
        );

        // ========================================
        // 2. SEED AMENITIES
        // ========================================
        modelBuilder.Entity<Amenity>().HasData(
            new Amenity { AmenityId = 1, AmenityName = "Swimming Pool", Description = "Outdoor swimming pool with lounge area", Icon = "pool-icon", IsActive = true, CreatedAt = DateTime.Now },
            new Amenity { AmenityId = 2, AmenityName = "Gym", Description = "24/7 fitness center with modern equipment", Icon = "gym-icon", IsActive = true, CreatedAt = DateTime.Now },
            new Amenity { AmenityId = 3, AmenityName = "Parking", Description = "Secure underground parking", Icon = "parking-icon", IsActive = true, CreatedAt = DateTime.Now },
            new Amenity { AmenityId = 4, AmenityName = "Concierge", Description = "24-hour concierge service", Icon = "concierge-icon", IsActive = true, CreatedAt = DateTime.Now },
            new Amenity { AmenityId = 5, AmenityName = "Rooftop Garden", Description = "Landscaped rooftop garden with city views", Icon = "garden-icon", IsActive = true, CreatedAt = DateTime.Now },
            new Amenity { AmenityId = 6, AmenityName = "Sauna", Description = "Traditional Finnish sauna", Icon = "sauna-icon", IsActive = true, CreatedAt = DateTime.Now },
            new Amenity { AmenityId = 7, AmenityName = "Pet Friendly", Description = "Pet-friendly building with pet spa", Icon = "pet-icon", IsActive = true, CreatedAt = DateTime.Now },
            new Amenity { AmenityId = 8, AmenityName = "Smart Home", Description = "Smart home automation system", Icon = "smart-icon", IsActive = true, CreatedAt = DateTime.Now },
            new Amenity { AmenityId = 9, AmenityName = "Laundry Room", Description = "On-site laundry facilities", Icon = "laundry-icon", IsActive = true, CreatedAt = DateTime.Now },
            new Amenity { AmenityId = 10, AmenityName = "Business Center", Description = "Co-working space and meeting rooms", Icon = "business-icon", IsActive = true, CreatedAt = DateTime.Now }
        );

        // ========================================
        // 3. SEED PROPERTIES
        // ========================================
        modelBuilder.Entity<Property>().HasData(
            new Property { PropertyId = 1, Name = "The Pearl Tower", Description = "Luxury residential tower", Address = "Building 123, Road 456", City = "Manama", PropertyType = "Residential", ImgPath = "/images/pearl-tower.jpg" },
            new Property { PropertyId = 2, Name = "Seef Views", Description = "Modern apartments with sea views", Address = "Seef District", City = "Manama", PropertyType = "Residential", ImgPath = "/images/seef-views.jpg" },
            new Property { PropertyId = 3, Name = "Amwaj Plaza", Description = "Waterfront living", Address = "Amwaj Islands", City = "Muharraq", PropertyType = "Residential", ImgPath = "/images/amwaj-plaza.jpg" },
            new Property { PropertyId = 4, Name = "Juffair Square", Description = "Central location, great amenities", Address = "Juffair", City = "Manama", PropertyType = "Residential", ImgPath = "/images/juffair-square.jpg" },
            new Property { PropertyId = 5, Name = "Diplomatic Heights", Description = "Premium diplomatic area", Address = "Diplomatic Area", City = "Manama", PropertyType = "Commercial", ImgPath = "/images/diplomatic-heights.jpg" },
            new Property { PropertyId = 6, Name = "Bahrain Bay Tower", Description = "Iconic waterfront property", Address = "Bahrain Bay", City = "Manama", PropertyType = "Residential", ImgPath = "/images/bahrain-bay.jpg" },
            new Property { PropertyId = 7, Name = "Riffa Views", Description = "Family-friendly community", Address = "East Riffa", City = "Riffa", PropertyType = "Residential", ImgPath = "/images/riffa-views.jpg" },
            new Property { PropertyId = 8, Name = "Saar Plaza", Description = "Suburban living", Address = "Saar", City = "Saar", PropertyType = "Residential", ImgPath = "/images/saar-plaza.jpg" },
            new Property { PropertyId = 9, Name = "Al Liwan Village", Description = "Mixed-use development", Address = "Hamala", City = "Hamala", PropertyType = "Commercial", ImgPath = "/images/al-liwan.jpg" },
            new Property { PropertyId = 10, Name = "Marassi Al Bahrain", Description = "Beachfront community", Address = "Diyar Al Muharraq", City = "Muharraq", PropertyType = "Residential", ImgPath = "/images/marassi.jpg" }
        );

        // ========================================
        // 4. SEED USERS
        // ========================================
        modelBuilder.Entity<User>().HasData(
            new User { UserId = 1, FullName = "John Smith", Email = "john.smith@example.com", Phone = "+97312345678", Role = "Tenant", SkillProfile = null, AvailabilityStatus = null, IdentityUserId = null },
            new User { UserId = 2, FullName = "Sarah Johnson", Email = "sarah.j@example.com", Phone = "+97312345679", Role = "Tenant", SkillProfile = null, AvailabilityStatus = null, IdentityUserId = null },
            new User { UserId = 3, FullName = "Mike Wilson", Email = "mike.w@example.com", Phone = "+97312345680", Role = "Tenant", SkillProfile = null, AvailabilityStatus = null, IdentityUserId = null },
            new User { UserId = 4, FullName = "Emma Brown", Email = "emma.b@example.com", Phone = "+97312345681", Role = "Tenant", SkillProfile = null, AvailabilityStatus = null, IdentityUserId = null },
            new User { UserId = 5, FullName = "David Lee", Email = "david.lee@example.com", Phone = "+97312345682", Role = "PropertyManager", SkillProfile = "Property Management", AvailabilityStatus = "Available", IdentityUserId = null },
            new User { UserId = 6, FullName = "Lisa Chen", Email = "lisa.c@example.com", Phone = "+97312345683", Role = "MaintenanceStaff", SkillProfile = "Plumbing, Electrical", AvailabilityStatus = "Available", IdentityUserId = null },
            new User { UserId = 7, FullName = "Robert Taylor", Email = "robert.t@example.com", Phone = "+97312345684", Role = "MaintenanceStaff", SkillProfile = "HVAC, General Repair", AvailabilityStatus = "Available", IdentityUserId = null },
            new User { UserId = 8, FullName = "Maria Garcia", Email = "maria.g@example.com", Phone = "+97312345685", Role = "PropertyManager", SkillProfile = "Leasing, Customer Service", AvailabilityStatus = "Available", IdentityUserId = null },
            new User { UserId = 9, FullName = "James Wilson", Email = "james.w@example.com", Phone = "+97312345686", Role = "Tenant", SkillProfile = null, AvailabilityStatus = null, IdentityUserId = null },
            new User { UserId = 10, FullName = "Patricia Moore", Email = "patricia.m@example.com", Phone = "+97312345687", Role = "MaintenanceStaff", SkillProfile = "Carpentry, Painting", AvailabilityStatus = "Busy", IdentityUserId = null }
        );

        // ========================================
        // 5. SEED UNITS
        // ========================================
        modelBuilder.Entity<Unit>().HasData(
            new Unit { UnitId = 1, PropertyId = 1, UnitNumber = "101", UnitType = "Apartment", Sizesqm = 85.5, MonthlyRent = 550.00m, AvailabilityStatus = "Available", ImgPath = "/images/unit101.jpg" },
            new Unit { UnitId = 2, PropertyId = 1, UnitNumber = "102", UnitType = "Apartment", Sizesqm = 95.0, MonthlyRent = 650.00m, AvailabilityStatus = "Occupied", ImgPath = "/images/unit102.jpg" },
            new Unit { UnitId = 3, PropertyId = 2, UnitNumber = "201", UnitType = "Studio", Sizesqm = 45.0, MonthlyRent = 400.00m, AvailabilityStatus = "Available", ImgPath = "/images/unit201.jpg" },
            new Unit { UnitId = 4, PropertyId = 2, UnitNumber = "202", UnitType = "Apartment", Sizesqm = 110.0, MonthlyRent = 800.00m, AvailabilityStatus = "Available", ImgPath = "/images/unit202.jpg" },
            new Unit { UnitId = 5, PropertyId = 3, UnitNumber = "301", UnitType = "Apartment", Sizesqm = 120.0, MonthlyRent = 950.00m, AvailabilityStatus = "Occupied", ImgPath = "/images/unit301.jpg" },
            new Unit { UnitId = 6, PropertyId = 3, UnitNumber = "302", UnitType = "Penthouse", Sizesqm = 200.0, MonthlyRent = 1800.00m, AvailabilityStatus = "Available", ImgPath = "/images/unit302.jpg" },
            new Unit { UnitId = 7, PropertyId = 4, UnitNumber = "401", UnitType = "Studio", Sizesqm = 40.0, MonthlyRent = 350.00m, AvailabilityStatus = "Available", ImgPath = "/images/unit401.jpg" },
            new Unit { UnitId = 8, PropertyId = 5, UnitNumber = "501", UnitType = "Office", Sizesqm = 150.0, MonthlyRent = 1200.00m, AvailabilityStatus = "Available", ImgPath = "/images/unit501.jpg" },
            new Unit { UnitId = 9, PropertyId = 6, UnitNumber = "601", UnitType = "Apartment", Sizesqm = 130.0, MonthlyRent = 1100.00m, AvailabilityStatus = "Occupied", ImgPath = "/images/unit601.jpg" },
            new Unit { UnitId = 10, PropertyId = 7, UnitNumber = "701", UnitType = "Townhouse", Sizesqm = 180.0, MonthlyRent = 1400.00m, AvailabilityStatus = "Available", ImgPath = "/images/unit701.jpg" }
        );

        // ========================================
        // 6. SEED LEASE APPLICATIONS
        // ========================================
        modelBuilder.Entity<LeaseApplication>().HasData(
            new LeaseApplication { ApplicationId = 1, UserId = 1, UnitId = 1, RequestedStartDate = DateTime.Now.AddDays(30), RequestedEndDate = DateTime.Now.AddYears(1).AddDays(30), Notes = "First-time renter, interested in 1-year lease", Status = "Approved", CreatedAt = DateTime.Now },
            new LeaseApplication { ApplicationId = 2, UserId = 2, UnitId = 3, RequestedStartDate = DateTime.Now.AddDays(15), RequestedEndDate = DateTime.Now.AddMonths(6).AddDays(15), Notes = "Short-term lease requested", Status = "Pending", CreatedAt = DateTime.Now },
            new LeaseApplication { ApplicationId = 3, UserId = 3, UnitId = 4, RequestedStartDate = DateTime.Now.AddDays(45), RequestedEndDate = DateTime.Now.AddYears(1).AddDays(45), Notes = "Family with 2 kids, need parking", Status = "Screening", CreatedAt = DateTime.Now },
            new LeaseApplication { ApplicationId = 4, UserId = 4, UnitId = 2, RequestedStartDate = DateTime.Now.AddDays(20), RequestedEndDate = DateTime.Now.AddYears(1).AddDays(20), Notes = "Professional couple", Status = "Approved", CreatedAt = DateTime.Now },
            new LeaseApplication { ApplicationId = 5, UserId = 5, UnitId = 5, RequestedStartDate = DateTime.Now.AddDays(60), RequestedEndDate = DateTime.Now.AddYears(2).AddDays(60), Notes = "Long-term lease preferred", Status = "Pending", CreatedAt = DateTime.Now },
            new LeaseApplication { ApplicationId = 6, UserId = 9, UnitId = 6, RequestedStartDate = DateTime.Now.AddDays(10), RequestedEndDate = DateTime.Now.AddYears(1).AddDays(10), Notes = "Immediate move-in", Status = "Approved", CreatedAt = DateTime.Now },
            new LeaseApplication { ApplicationId = 7, UserId = 1, UnitId = 7, RequestedStartDate = DateTime.Now.AddDays(90), RequestedEndDate = DateTime.Now.AddYears(1).AddDays(90), Notes = "Looking for quiet location", Status = "Rejected", CreatedAt = DateTime.Now },
            new LeaseApplication { ApplicationId = 8, UserId = 2, UnitId = 8, RequestedStartDate = DateTime.Now.AddDays(25), RequestedEndDate = DateTime.Now.AddMonths(9).AddDays(25), Notes = "Business professional", Status = "Screening", CreatedAt = DateTime.Now },
            new LeaseApplication { ApplicationId = 9, UserId = 3, UnitId = 9, RequestedStartDate = DateTime.Now.AddDays(35), RequestedEndDate = DateTime.Now.AddYears(1).AddDays(35), Notes = "Working from home", Status = "Pending", CreatedAt = DateTime.Now },
            new LeaseApplication { ApplicationId = 10, UserId = 4, UnitId = 10, RequestedStartDate = DateTime.Now.AddDays(50), RequestedEndDate = DateTime.Now.AddYears(2).AddDays(50), Notes = "Need parking included", Status = "Approved", CreatedAt = DateTime.Now }
        );

        // ========================================
        // 7. SEED LEASES
        // ========================================
        modelBuilder.Entity<Lease>().HasData(
            new Lease { LeaseId = 1, ApplicationId = 1, LeaseStartDate = DateTime.Now.AddDays(30), LeaseEndDate = DateTime.Now.AddYears(1).AddDays(30), MonthlyRent = 550.00m, SecurityDeposit = 550.00m, CreatedAt = DateTime.Now, ParentLeaseId = null, TerminationReason = null, TerminationDate = null },
            new Lease { LeaseId = 2, ApplicationId = 4, LeaseStartDate = DateTime.Now.AddDays(20), LeaseEndDate = DateTime.Now.AddYears(1).AddDays(20), MonthlyRent = 650.00m, SecurityDeposit = 650.00m, CreatedAt = DateTime.Now, ParentLeaseId = null, TerminationReason = null, TerminationDate = null },
            new Lease { LeaseId = 3, ApplicationId = 6, LeaseStartDate = DateTime.Now.AddDays(10), LeaseEndDate = DateTime.Now.AddYears(1).AddDays(10), MonthlyRent = 1100.00m, SecurityDeposit = 1100.00m, CreatedAt = DateTime.Now, ParentLeaseId = null, TerminationReason = null, TerminationDate = null },
            new Lease { LeaseId = 4, ApplicationId = 10, LeaseStartDate = DateTime.Now.AddDays(50), LeaseEndDate = DateTime.Now.AddYears(2).AddDays(50), MonthlyRent = 1400.00m, SecurityDeposit = 1400.00m, CreatedAt = DateTime.Now, ParentLeaseId = null, TerminationReason = null, TerminationDate = null },
            new Lease { LeaseId = 5, ApplicationId = 2, LeaseStartDate = DateTime.Now.AddDays(15), LeaseEndDate = DateTime.Now.AddMonths(6).AddDays(15), MonthlyRent = 400.00m, SecurityDeposit = 400.00m, CreatedAt = DateTime.Now, ParentLeaseId = null, TerminationReason = null, TerminationDate = null },
            new Lease { LeaseId = 6, ApplicationId = 5, LeaseStartDate = DateTime.Now.AddDays(60), LeaseEndDate = DateTime.Now.AddYears(2).AddDays(60), MonthlyRent = 1200.00m, SecurityDeposit = 1200.00m, CreatedAt = DateTime.Now, ParentLeaseId = null, TerminationReason = null, TerminationDate = null },
            new Lease { LeaseId = 7, ApplicationId = 9, LeaseStartDate = DateTime.Now.AddDays(35), LeaseEndDate = DateTime.Now.AddYears(1).AddDays(35), MonthlyRent = 800.00m, SecurityDeposit = 800.00m, CreatedAt = DateTime.Now, ParentLeaseId = null, TerminationReason = null, TerminationDate = null },
            new Lease { LeaseId = 8, ApplicationId = 3, LeaseStartDate = DateTime.Now.AddDays(45), LeaseEndDate = DateTime.Now.AddYears(1).AddDays(45), MonthlyRent = 950.00m, SecurityDeposit = 950.00m, CreatedAt = DateTime.Now, ParentLeaseId = null, TerminationReason = null, TerminationDate = null },
            new Lease { LeaseId = 9, ApplicationId = 8, LeaseStartDate = DateTime.Now.AddDays(25), LeaseEndDate = DateTime.Now.AddMonths(9).AddDays(25), MonthlyRent = 350.00m, SecurityDeposit = 350.00m, CreatedAt = DateTime.Now, ParentLeaseId = null, TerminationReason = null, TerminationDate = null },
            new Lease { LeaseId = 10, ApplicationId = 7, LeaseStartDate = DateTime.Now.AddDays(90), LeaseEndDate = DateTime.Now.AddYears(1).AddDays(90), MonthlyRent = 1800.00m, SecurityDeposit = 1800.00m, CreatedAt = DateTime.Now, ParentLeaseId = null, TerminationReason = null, TerminationDate = null }
        );

        // ========================================
        // 8. SEED MAINTENANCE REQUESTS
        // ========================================
        modelBuilder.Entity<MaintenanceRequest>().HasData(
            new MaintenanceRequest { RequestId = 1, UnitId = 1, TenantUserId = 1, AssignedStaffId = null, Title = "AC not working", Description = "Air conditioning unit making strange noises and not cooling", RequestType = "HVAC", Priority = "High", StatusId = 1, TicketNumber = "REQ-001", ChangedByUserId = null, SubmittedAt = DateTime.Now.AddDays(-5), ResolvedAt = null, ResolutionNotes = null },
            new MaintenanceRequest { RequestId = 2, UnitId = 2, TenantUserId = 2, AssignedStaffId = 7, Title = "Leaking faucet", Description = "Kitchen faucet leaking constantly", RequestType = "Plumbing", Priority = "Medium", StatusId = 2, TicketNumber = "REQ-002", ChangedByUserId = null, SubmittedAt = DateTime.Now.AddDays(-3), ResolvedAt = null, ResolutionNotes = null },
            new MaintenanceRequest { RequestId = 3, UnitId = 3, TenantUserId = 3, AssignedStaffId = 6, Title = "Electrical issue", Description = "Light switch not working in bedroom", RequestType = "Electrical", Priority = "Medium", StatusId = 3, TicketNumber = "REQ-003", ChangedByUserId = null, SubmittedAt = DateTime.Now.AddDays(-7), ResolvedAt = null, ResolutionNotes = null },
            new MaintenanceRequest { RequestId = 4, UnitId = 4, TenantUserId = 4, AssignedStaffId = 7, Title = "Clogged drain", Description = "Shower drain clogged and slow", RequestType = "Plumbing", Priority = "Low", StatusId = 4, TicketNumber = "REQ-004", ChangedByUserId = null, SubmittedAt = DateTime.Now.AddDays(-10), ResolvedAt = DateTime.Now.AddDays(-8), ResolutionNotes = "Drain cleared successfully" },
            new MaintenanceRequest { RequestId = 5, UnitId = 5, TenantUserId = 5, AssignedStaffId = null, Title = "Painting needed", Description = "Wall paint peeling in living room", RequestType = "Maintenance", Priority = "Low", StatusId = 1, TicketNumber = "REQ-005", ChangedByUserId = null, SubmittedAt = DateTime.Now.AddDays(-2), ResolvedAt = null, ResolutionNotes = null },
            new MaintenanceRequest { RequestId = 6, UnitId = 6, TenantUserId = 9, AssignedStaffId = 10, Title = "Window broken", Description = "Living room window cracked", RequestType = "Repair", Priority = "High", StatusId = 2, TicketNumber = "REQ-006", ChangedByUserId = null, SubmittedAt = DateTime.Now.AddDays(-1), ResolvedAt = null, ResolutionNotes = null },
            new MaintenanceRequest { RequestId = 7, UnitId = 7, TenantUserId = 1, AssignedStaffId = 6, Title = "Door lock issue", Description = "Front door lock sticking", RequestType = "Security", Priority = "Medium", StatusId = 3, TicketNumber = "REQ-007", ChangedByUserId = null, SubmittedAt = DateTime.Now.AddDays(-8), ResolvedAt = null, ResolutionNotes = null },
            new MaintenanceRequest { RequestId = 8, UnitId = 8, TenantUserId = 2, AssignedStaffId = null, Title = "Pest control", Description = "Ants in kitchen area", RequestType = "Cleaning", Priority = "Medium", StatusId = 1, TicketNumber = "REQ-008", ChangedByUserId = null, SubmittedAt = DateTime.Now.AddDays(-4), ResolvedAt = null, ResolutionNotes = null },
            new MaintenanceRequest { RequestId = 9, UnitId = 9, TenantUserId = 3, AssignedStaffId = 10, Title = "Water heater", Description = "No hot water", RequestType = "Plumbing", Priority = "High", StatusId = 4, TicketNumber = "REQ-009", ChangedByUserId = null, SubmittedAt = DateTime.Now.AddDays(-12), ResolvedAt = DateTime.Now.AddDays(-10), ResolutionNotes = "Heating element replaced" },
            new MaintenanceRequest { RequestId = 10, UnitId = 10, TenantUserId = 4, AssignedStaffId = 7, Title = "Carpet cleaning", Description = "Carpet stained and needs professional cleaning", RequestType = "Cleaning", Priority = "Low", StatusId = 1, TicketNumber = "REQ-010", ChangedByUserId = null, SubmittedAt = DateTime.Now.AddDays(-6), ResolvedAt = null, ResolutionNotes = null }
        );

        // ========================================
        // 9. SEED PAYMENT RECORDS
        // ========================================
        modelBuilder.Entity<PaymentRecord>().HasData(
            new PaymentRecord { PaymentId = 1, LeaseId = 1, AmountDue = 550.00m, AmountPaid = 550.00m, DueDate = DateTime.Now.AddDays(-5), PaidDate = DateTime.Now.AddDays(-5), PaymentStatus = "Paid", Notes = "January rent" },
            new PaymentRecord { PaymentId = 2, LeaseId = 1, AmountDue = 550.00m, AmountPaid = null, DueDate = DateTime.Now.AddDays(25), PaidDate = null, PaymentStatus = "Pending", Notes = "February rent" },
            new PaymentRecord { PaymentId = 3, LeaseId = 2, AmountDue = 650.00m, AmountPaid = 650.00m, DueDate = DateTime.Now.AddDays(-10), PaidDate = DateTime.Now.AddDays(-10), PaymentStatus = "Paid", Notes = "January rent" },
            new PaymentRecord { PaymentId = 4, LeaseId = 3, AmountDue = 1100.00m, AmountPaid = 1100.00m, DueDate = DateTime.Now.AddDays(-15), PaidDate = DateTime.Now.AddDays(-15), PaymentStatus = "Paid", Notes = "January rent" },
            new PaymentRecord { PaymentId = 5, LeaseId = 3, AmountDue = 1100.00m, AmountPaid = 500.00m, DueDate = DateTime.Now.AddDays(15), PaidDate = null, PaymentStatus = "Partial", Notes = "Partial payment received" },
            new PaymentRecord { PaymentId = 6, LeaseId = 4, AmountDue = 1400.00m, AmountPaid = null, DueDate = DateTime.Now.AddDays(-3), PaidDate = null, PaymentStatus = "Overdue", Notes = "Late payment" },
            new PaymentRecord { PaymentId = 7, LeaseId = 5, AmountDue = 400.00m, AmountPaid = 400.00m, DueDate = DateTime.Now.AddDays(-20), PaidDate = DateTime.Now.AddDays(-20), PaymentStatus = "Paid", Notes = "January rent" },
            new PaymentRecord { PaymentId = 8, LeaseId = 6, AmountDue = 1200.00m, AmountPaid = null, DueDate = DateTime.Now.AddDays(10), PaidDate = null, PaymentStatus = "Pending", Notes = "First month rent" },
            new PaymentRecord { PaymentId = 9, LeaseId = 7, AmountDue = 800.00m, AmountPaid = 800.00m, DueDate = DateTime.Now.AddDays(-8), PaidDate = DateTime.Now.AddDays(-8), PaymentStatus = "Paid", Notes = "January rent" },
            new PaymentRecord { PaymentId = 10, LeaseId = 8, AmountDue = 950.00m, AmountPaid = null, DueDate = DateTime.Now.AddDays(30), PaidDate = null, PaymentStatus = "Pending", Notes = "Upcoming payment" }
        );

        // ========================================
        // 10. SEED UNIT AMENITIES
        // ========================================
        modelBuilder.Entity<UnitAmenity>().HasData(
            new UnitAmenity { UnitAmenityId = 1, UnitId = 1, AmenityId = 1, IsActive = true, CreatedAt = DateTime.Now },
            new UnitAmenity { UnitAmenityId = 2, UnitId = 1, AmenityId = 2, IsActive = true, CreatedAt = DateTime.Now },
            new UnitAmenity { UnitAmenityId = 3, UnitId = 1, AmenityId = 3, IsActive = true, CreatedAt = DateTime.Now },
            new UnitAmenity { UnitAmenityId = 4, UnitId = 2, AmenityId = 1, IsActive = true, CreatedAt = DateTime.Now },
            new UnitAmenity { UnitAmenityId = 5, UnitId = 3, AmenityId = 2, IsActive = true, CreatedAt = DateTime.Now },
            new UnitAmenity { UnitAmenityId = 6, UnitId = 4, AmenityId = 1, IsActive = true, CreatedAt = DateTime.Now },
            new UnitAmenity { UnitAmenityId = 7, UnitId = 4, AmenityId = 7, IsActive = true, CreatedAt = DateTime.Now },
            new UnitAmenity { UnitAmenityId = 8, UnitId = 5, AmenityId = 1, IsActive = true, CreatedAt = DateTime.Now },
            new UnitAmenity { UnitAmenityId = 9, UnitId = 5, AmenityId = 2, IsActive = true, CreatedAt = DateTime.Now },
            new UnitAmenity { UnitAmenityId = 10, UnitId = 5, AmenityId = 8, IsActive = true, CreatedAt = DateTime.Now }
        );

        // ========================================
        // 11. SEED NOTIFICATIONS
        // ========================================
        modelBuilder.Entity<Notification>().HasData(
            new Notification { NotificationId = 1, UserId = 1, Message = "Your lease application has been approved!", NotificationType = "LeaseUpdate", Status = "Read", CreatedAt = DateTime.Now.AddDays(-20) },
            new Notification { NotificationId = 2, UserId = 2, Message = "Maintenance request #REQ-002 has been assigned", NotificationType = "MaintenanceUpdate", Status = "Read", CreatedAt = DateTime.Now.AddDays(-3) },
            new Notification { NotificationId = 3, UserId = 3, Message = "Rent payment due in 5 days", NotificationType = "PaymentReminder", Status = "Unread", CreatedAt = DateTime.Now.AddDays(-2) },
            new Notification { NotificationId = 4, UserId = 4, Message = "Your unit inspection is scheduled for next week", NotificationType = "General", Status = "Read", CreatedAt = DateTime.Now.AddDays(-7) },
            new Notification { NotificationId = 5, UserId = 5, Message = "New lease application received for Pearl Tower Unit 101", NotificationType = "LeaseApplication", Status = "Read", CreatedAt = DateTime.Now.AddDays(-15) },
            new Notification { NotificationId = 6, UserId = 6, Message = "You have been assigned to maintenance request #REQ-003", NotificationType = "MaintenanceUpdate", Status = "Unread", CreatedAt = DateTime.Now.AddDays(-7) },
            new Notification { NotificationId = 7, UserId = 7, Message = "Maintenance request #REQ-004 has been resolved", NotificationType = "MaintenanceUpdate", Status = "Read", CreatedAt = DateTime.Now.AddDays(-10) },
            new Notification { NotificationId = 8, UserId = 8, Message = "Welcome to Property Leasing System!", NotificationType = "General", Status = "Read", CreatedAt = DateTime.Now.AddDays(-30) },
            new Notification { NotificationId = 9, UserId = 9, Message = "Your rent payment is now overdue", NotificationType = "PaymentReminder", Status = "Unread", CreatedAt = DateTime.Now.AddDays(-3) },
            new Notification { NotificationId = 10, UserId = 10, Message = "New maintenance request submitted for your property", NotificationType = "General", Status = "Read", CreatedAt = DateTime.Now.AddDays(-6) }
        );

        // ========================================
        // 12. SEED FEEDBACKS
        // ========================================
        modelBuilder.Entity<Feedback>().HasData(
            new Feedback { FeedbackId = 1, UserId = 1, UnitId = 1, Rating = 5, Comment = "Great apartment, very clean and well-maintained!", IsVisible = true, CreatedAt = DateTime.Now.AddDays(-15) },
            new Feedback { FeedbackId = 2, UserId = 2, UnitId = 3, Rating = 4, Comment = "Good value for money. AC works well.", IsVisible = true, CreatedAt = DateTime.Now.AddDays(-10) },
            new Feedback { FeedbackId = 3, UserId = 3, UnitId = 4, Rating = 3, Comment = "Decent place but parking is tight", IsVisible = true, CreatedAt = DateTime.Now.AddDays(-8) },
            new Feedback { FeedbackId = 4, UserId = 4, UnitId = 2, Rating = 5, Comment = "Excellent location and responsive management", IsVisible = true, CreatedAt = DateTime.Now.AddDays(-20) },
            new Feedback { FeedbackId = 5, UserId = 5, UnitId = 5, Rating = 4, Comment = "Professional office space, good amenities", IsVisible = true, CreatedAt = DateTime.Now.AddDays(-12) },
            new Feedback { FeedbackId = 6, UserId = 9, UnitId = 6, Rating = 2, Comment = "Had maintenance issues that took too long", IsVisible = true, CreatedAt = DateTime.Now.AddDays(-5) },
            new Feedback { FeedbackId = 7, UserId = 1, UnitId = 7, Rating = 4, Comment = "Quiet neighborhood, friendly neighbors", IsVisible = true, CreatedAt = DateTime.Now.AddDays(-18) },
            new Feedback { FeedbackId = 8, UserId = 2, UnitId = 8, Rating = 5, Comment = "Perfect for business, would recommend", IsVisible = true, CreatedAt = DateTime.Now.AddDays(-14) },
            new Feedback { FeedbackId = 9, UserId = 3, UnitId = 9, Rating = 3, Comment = "Good but internet connection issues", IsVisible = true, CreatedAt = DateTime.Now.AddDays(-7) },
            new Feedback { FeedbackId = 10, UserId = 4, UnitId = 10, Rating = 4, Comment = "Spacious and modern design", IsVisible = true, CreatedAt = DateTime.Now.AddDays(-9) }
        );

        // ========================================
        // 13. SEED LOGS
        // ========================================
        modelBuilder.Entity<Log>().HasData(
            new Log { LogId = 1, UserId = 5, Action = "Login", Details = "User logged in successfully", LogLevel = "Info", Source = "Web", CreatedAt = DateTime.Now.AddDays(-5) },
            new Log { LogId = 2, UserId = 5, Action = "Create Lease", Details = "Created lease for Unit 101", LogLevel = "Info", Source = "API", CreatedAt = DateTime.Now.AddDays(-4) },
            new Log { LogId = 3, UserId = 6, Action = "Update Request", Details = "Updated maintenance request status", LogLevel = "Info", Source = "Web", CreatedAt = DateTime.Now.AddDays(-3) },
            new Log { LogId = 4, UserId = 7, Action = "Login Failed", Details = "Failed login attempt", LogLevel = "Warning", Source = "Web", CreatedAt = DateTime.Now.AddDays(-2) },
            new Log { LogId = 5, UserId = 8, Action = "Generate Report", Details = "Generated occupancy report", LogLevel = "Info", Source = "API", CreatedAt = DateTime.Now.AddDays(-1) },
            new Log { LogId = 6, UserId = 1, Action = "Submit Application", Details = "Submitted lease application", LogLevel = "Info", Source = "Web", CreatedAt = DateTime.Now.AddDays(-10) },
            new Log { LogId = 7, UserId = 2, Action = "Login", Details = "User logged in", LogLevel = "Info", Source = "Web", CreatedAt = DateTime.Now.AddDays(-8) },
            new Log { LogId = 8, UserId = 3, Action = "Payment", Details = "Processed rent payment", LogLevel = "Info", Source = "API", CreatedAt = DateTime.Now.AddDays(-6) },
            new Log { LogId = 9, UserId = 4, Action = "View Property", Details = "Viewed property details", LogLevel = "Info", Source = "Web", CreatedAt = DateTime.Now.AddDays(-4) },
            new Log { LogId = 10, UserId = 9, Action = "Logout", Details = "User logged out", LogLevel = "Info", Source = "Web", CreatedAt = DateTime.Now.AddDays(-2) }
        );
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}