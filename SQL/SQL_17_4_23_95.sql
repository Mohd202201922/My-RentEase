-- ==================================================
-- Drop and recreate database 'PropertyLeasingDB'
-- Requirements:
-- ✓ Database name: PropertyLeasingDB (from your files)
-- ✓ No IDENTITY columns → Using UNIQUEIDENTIFIER + DEFAULT NEWID()
-- ✓ No ASP.NET Identity integration
-- ✓ No Skills/StaffSkill tables
-- ✓ MaintenanceStaff table: (StaffId, UserId, Category, CreatedAt)
-- ✓ MaintenanceRequests: EXACTLY 8 columns as specified
-- ✓ Properties table (not Buildings)
-- ==================================================

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'PropertyLeasingDB')
BEGIN
    ALTER DATABASE PropertyLeasingDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PropertyLeasingDB;
END;
GO

CREATE DATABASE PropertyLeasingDB;
GO

USE PropertyLeasingDB;
GO

-- ============================================
-- CORE TABLES
-- ============================================

-- Users (All roles: Tenant, PropertyManager, MaintenanceStaff)
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(256) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20) NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Tenant', 'PropertyManager', 'MaintenanceStaff')),
    IsActive BIT NOT NULL DEFAULT 1,
    AvailabilityStatus NVARCHAR(20) NULL CHECK (AvailabilityStatus IN ('Available', 'Busy', 'Off')),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- MaintenanceStaff (Profile extension for MaintenanceStaff role users)
CREATE TABLE MaintenanceStaff (
    StaffId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    Category NVARCHAR(50) NOT NULL, -- e.g., 'Plumbing', 'Electrical', 'HVAC', 'General'
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Properties (Buildings/Complexes - renamed from Property to match plural convention)
CREATE TABLE Properties (
    PropertyId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(250) NULL,
    Address NVARCHAR(200) NOT NULL,
    City NVARCHAR(50) NULL,
    PropertyType NVARCHAR(50) NULL CHECK (PropertyType IN ('Residential', 'Commercial')),
    ImgPath NVARCHAR(100) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Units (Individual rental units within Properties)
CREATE TABLE Units (
    UnitId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    PropertyId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Properties(PropertyId),
    UnitNumber NVARCHAR(50) NOT NULL,
    Type NVARCHAR(50) NOT NULL CHECK (Type IN ('Apartment', 'Studio', 'Office', 'Shop', 'Residential', 'Commercial')),
    Size DECIMAL(10,2) NULL, -- in sqm
    RentAmount DECIMAL(18,2) NOT NULL,
    Amenities NVARCHAR(250) NULL, -- Comma-separated or use UnitAmenity junction
    AvailabilityStatus NVARCHAR(20) NOT NULL CHECK (AvailabilityStatus IN ('Available', 'Occupied', 'UnderMaintenance', 'Leased', 'Maintenance')),
    ImgPath NVARCHAR(100) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Amenities (Master list - optional, if you want normalized amenities)
CREATE TABLE Amenities (
    AmenityId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(50) NOT NULL UNIQUE
);

-- UnitAmenity (Junction table - optional, if using normalized Amenities)
CREATE TABLE UnitAmenity (
    UnitId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Units(UnitId),
    AmenityId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Amenities(AmenityId),
    PRIMARY KEY (UnitId, AmenityId)
);

-- ============================================
-- LEASING LIFECYCLE TABLES
-- ============================================

-- LeaseApplications (Tenant applies for a unit)
CREATE TABLE LeaseApplications (
    ApplicationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UnitId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Units(UnitId),
    TenantId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    RequestedStartDate DATE NULL,
    RequestedEndDate DATE NULL,
    Notes NVARCHAR(500) NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Pending', 'Screening', 'Approved', 'Rejected')),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- ScreeningUnit (Screening appointment for approved applications)
CREATE TABLE ScreeningUnit (
    ScreeningUnitId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    LeaseApplicationId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES LeaseApplications(ApplicationId),
    ScreeningAppointmentTime DATETIME2 NOT NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Canceled', 'Approved', 'Rejected')),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- LeaseAgreements (Active lease contracts)
CREATE TABLE LeaseAgreements (
    LeaseId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ApplicationId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES LeaseApplications(ApplicationId),
    UnitId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Units(UnitId),
    TenantId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    RentAmount DECIMAL(18,2) NOT NULL,
    SecurityDeposit DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Active', 'Expired', 'Terminated', 'Renewed')),
    TerminationReason NVARCHAR(200) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT CK_LeaseAgreements_DateRange CHECK (EndDate >= StartDate)
);

-- Payments (Lease payment installments)
CREATE TABLE Payments (
    PaymentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    LeaseId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES LeaseAgreements(LeaseId),
    AmountDue DECIMAL(18,2) NOT NULL,
    AmountPaid DECIMAL(18,2) NULL,
    DueDate DATE NOT NULL,
    PaidDate DATE NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Pending', 'Paid', 'Overdue', 'Partial')),
    PaymentMethod NVARCHAR(50) NULL,
    Notes NVARCHAR(250) NULL,
    RecordedBy UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    RecordedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- ============================================
-- MAINTENANCE TABLES
-- ============================================

-- MaintenanceRequests (EXACTLY 8 columns as you specified)
CREATE TABLE MaintenanceRequests (
    RequestId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UnitId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Units(UnitId),
    TenantId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    AssignedTo UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Users(Id),
    Category NVARCHAR(50) NOT NULL, -- e.g., 'Plumbing', 'Electrical', 'HVAC', 'General'
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Submitted', 'Assigned', 'In Progress', 'Resolved', 'Closed')),
    Description NVARCHAR(1000) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- MaintenanceStatusHistory (Optional: Track status changes for audit)
CREATE TABLE MaintenanceStatusHistory (
    HistoryId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RequestId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES MaintenanceRequests(RequestId),
    OldStatus NVARCHAR(20) NULL,
    NewStatus NVARCHAR(20) NOT NULL,
    Notes NVARCHAR(250) NULL,
    ChangedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ChangedByUserId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Users(Id)
);

-- ============================================
-- NOTIFICATIONS & AUXILIARY TABLES
-- ============================================

CREATE TABLE Notifications (
    NotificationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    Message NVARCHAR(500) NOT NULL,
    NotificationType NVARCHAR(50) NULL CHECK (NotificationType IN ('MaintenanceUpdate', 'LeaseApplication', 'PaymentReminder', 'General')),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Unread' CHECK (Status IN ('Read', 'Unread')),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    RelatedEntityType NVARCHAR(50) NULL,
    RelatedEntityId UNIQUEIDENTIFIER NULL
);

-- Feedback (Optional: Tenant reviews)
CREATE TABLE Feedback (
    FeedbackId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Users(Id),
    UnitId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Units(UnitId),
    Rating INT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(500) NULL,
    IsVisible BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Log (Optional: Audit trail)
CREATE TABLE Logs (
    LogId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NULL FOREIGN KEY REFERENCES Users(Id),
    Action NVARCHAR(100) NULL,
    Details NVARCHAR(500) NULL,
    LogLevel NVARCHAR(20) NULL CHECK (LogLevel IN ('Info', 'Warning', 'Error')),
    Source NVARCHAR(20) NULL CHECK (Source IN ('Web', 'API')),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- ============================================
-- INDEXES (For performance)
-- ============================================

CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Role ON Users(Role);
CREATE INDEX IX_Users_IsActive ON Users(IsActive);

CREATE INDEX IX_MaintenanceStaff_UserId ON MaintenanceStaff(UserId);
CREATE INDEX IX_MaintenanceStaff_Category ON MaintenanceStaff(Category);

CREATE INDEX IX_Properties_City ON Properties(City);
CREATE INDEX IX_Properties_Type ON Properties(PropertyType);

CREATE INDEX IX_Units_PropertyId ON Units(PropertyId);
CREATE INDEX IX_Units_AvailabilityStatus ON Units(AvailabilityStatus);
CREATE INDEX IX_Units_Type ON Units(Type);

CREATE INDEX IX_LeaseApplications_UnitId ON LeaseApplications(UnitId);
CREATE INDEX IX_LeaseApplications_TenantId ON LeaseApplications(TenantId);
CREATE INDEX IX_LeaseApplications_Status ON LeaseApplications(Status);

CREATE INDEX IX_ScreeningUnit_LeaseApplicationId ON ScreeningUnit(LeaseApplicationId);
CREATE INDEX IX_ScreeningUnit_Status ON ScreeningUnit(Status);

CREATE INDEX IX_LeaseAgreements_UnitId ON LeaseAgreements(UnitId);
CREATE INDEX IX_LeaseAgreements_TenantId ON LeaseAgreements(TenantId);
CREATE INDEX IX_LeaseAgreements_Status ON LeaseAgreements(Status);
CREATE INDEX IX_LeaseAgreements_DateRange ON LeaseAgreements(StartDate, EndDate);

CREATE INDEX IX_Payments_LeaseId ON Payments(LeaseId);
CREATE INDEX IX_Payments_Status ON Payments(Status);
CREATE INDEX IX_Payments_DueDate ON Payments(DueDate);

CREATE INDEX IX_MaintenanceRequests_UnitId ON MaintenanceRequests(UnitId);
CREATE INDEX IX_MaintenanceRequests_TenantId ON MaintenanceRequests(TenantId);
CREATE INDEX IX_MaintenanceRequests_AssignedTo ON MaintenanceRequests(AssignedTo);
CREATE INDEX IX_MaintenanceRequests_Status ON MaintenanceRequests(Status);
CREATE INDEX IX_MaintenanceRequests_Category ON MaintenanceRequests(Category);

CREATE INDEX IX_Notifications_UserId ON Notifications(UserId, Status);
CREATE INDEX IX_Notifications_CreatedAt ON Notifications(CreatedAt);

CREATE INDEX IX_MaintenanceStatusHistory_RequestId ON MaintenanceStatusHistory(RequestId);
GO

-- ============================================
-- MINIMAL SEED DATA (Using NEWID() for GUIDs)
-- ============================================

-- Users
DECLARE @Tenant1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Tenant2 UNIQUEIDENTIFIER = NEWID();
DECLARE @Manager UNIQUEIDENTIFIER = NEWID();
DECLARE @Staff1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Staff2 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Users (Id, Email, PasswordHash, FirstName, LastName, PhoneNumber, Role, AvailabilityStatus)
VALUES 
    (@Tenant1, 'tenant1@example.com', '$2a$11$placeholder_hash', 'Ahmed', 'Al Mansoori', '+973 3300 0002', 'Tenant', NULL),
    (@Tenant2, 'tenant2@example.com', '$2a$11$placeholder_hash', 'Mohammed', 'Al Tajer', '+973 3300 0003', 'Tenant', NULL),
    (@Manager, 'manager@propleasing.com', '$2a$11$placeholder_hash', 'Sara', 'Al Khalifa', '+973 3300 0001', 'PropertyManager', NULL),
    (@Staff1, 'staff1@propleasing.com', '$2a$11$placeholder_hash', 'Ali', 'Hassan', '+973 3300 0004', 'MaintenanceStaff', 'Available'),
    (@Staff2, 'staff2@propleasing.com', '$2a$11$placeholder_hash', 'Yusuf', 'Al Zayani', '+973 3300 0005', 'MaintenanceStaff', 'Available');

-- MaintenanceStaff profiles
INSERT INTO MaintenanceStaff (UserId, Category) 
VALUES (@Staff1, 'Plumbing'), (@Staff1, 'General'), (@Staff2, 'Electrical'), (@Staff2, 'HVAC');

-- Properties
DECLARE @Prop1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Prop2 UNIQUEIDENTIFIER = NEWID();
INSERT INTO Properties (PropertyId, Name, Description, Address, City, PropertyType)
VALUES 
    (@Prop1, 'Seef Tower', 'Modern residential tower near Seef Mall', 'Building 101, Seef District', 'Manama', 'Residential'),
    (@Prop2, 'Gulf Business Center', 'Commercial offices in central Manama', 'Road 25, Diplomatic Area', 'Manama', 'Commercial');

-- Units
DECLARE @Unit1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Unit2 UNIQUEIDENTIFIER = NEWID();
DECLARE @Unit3 UNIQUEIDENTIFIER = NEWID();
INSERT INTO Units (UnitId, PropertyId, UnitNumber, Type, Size, RentAmount, Amenities, AvailabilityStatus)
VALUES 
    (@Unit1, @Prop1, '101', 'Apartment', 85.00, 450.00, 'Gym, Pool, Parking', 'Available'),
    (@Unit2, @Prop1, '102', 'Studio', 45.00, 280.00, 'Parking', 'Available'),
    (@Unit3, @Prop2, 'G01', 'Office', 120.00, 800.00, 'Meeting Rooms, Parking', 'Available');

-- Lease Application Flow
DECLARE @AppId UNIQUEIDENTIFIER = NEWID();
INSERT INTO LeaseApplications (ApplicationId, UnitId, TenantId, Status, CreatedAt) 
VALUES (@AppId, @Unit1, @Tenant1, 'Approved', GETUTCDATE());

DECLARE @ScreeningId UNIQUEIDENTIFIER = NEWID();
INSERT INTO ScreeningUnit (ScreeningUnitId, LeaseApplicationId, ScreeningAppointmentTime, Status)
VALUES (@ScreeningId, @AppId, DATEADD(day, 2, GETUTCDATE()), 'Approved');

DECLARE @LeaseId UNIQUEIDENTIFIER = NEWID();
INSERT INTO LeaseAgreements (LeaseId, ApplicationId, UnitId, TenantId, StartDate, EndDate, RentAmount, SecurityDeposit, Status)
VALUES (@LeaseId, @AppId, @Unit1, @Tenant1, '2026-06-01', '2027-05-31', 450.00, 900.00, 'Active');

-- Update unit status
UPDATE Units SET AvailabilityStatus = 'Occupied' WHERE UnitId = @Unit1;

-- Payment
INSERT INTO Payments (LeaseId, AmountDue, DueDate, Status, RecordedBy)
VALUES (@LeaseId, 450.00, '2026-06-01', 'Pending', @Manager);

-- Maintenance Request (EXACTLY 8 columns - Title is merged into Description)
DECLARE @ReqId UNIQUEIDENTIFIER = NEWID();
INSERT INTO MaintenanceRequests (RequestId, UnitId, TenantId, Category, Status, Description)
VALUES (@ReqId, @Unit1, @Tenant1, 'Plumbing', 'Submitted', 'Leaking faucet in kitchen - Unit 101, Seef Tower');

-- Notification
INSERT INTO Notifications (UserId, Message, NotificationType, RelatedEntityType, RelatedEntityId)
VALUES (@Tenant1, 'Your maintenance request has been submitted successfully.', 'MaintenanceUpdate', 'MaintenanceRequest', @ReqId);

-- Maintenance Status History
INSERT INTO MaintenanceStatusHistory (RequestId, OldStatus, NewStatus, Notes, ChangedByUserId)
VALUES (@ReqId, NULL, 'Submitted', 'Request created by tenant', @Tenant1);

-- Feedback
INSERT INTO Feedback (UserId, UnitId, Rating, Comment)
VALUES (@Tenant1, @Unit1, 5, 'Great apartment, very clean and well-maintained!');

-- Log
INSERT INTO Logs (UserId, Action, Details, LogLevel, Source)
VALUES (@Manager, 'CreateLease', 'Lease created for ApplicationId=' + CAST(@AppId AS NVARCHAR(36)), 'Info', 'Web');

PRINT 'PropertyLeasingDB schema and seed data created successfully!';
GO