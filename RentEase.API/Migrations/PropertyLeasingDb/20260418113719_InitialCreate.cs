using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RentEase.API.Migrations.PropertyLeasingDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    AmenityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmenityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Amenitie__842AF52B66D7181C", x => x.AmenityID);
                });

            migrationBuilder.CreateTable(
                name: "LeaseApplicationStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsFinal = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LeaseApp__C8EE20437A0C0CE2", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "LeaseStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsTerminal = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LeaseSta__C8EE2043F72AD14C", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRequestStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Maintena__C8EE204319ED2289", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    PropertyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PropertyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ImgPath = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Property__70C9A75539279CAD", x => x.PropertyID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Tenant"),
                    SkillProfile = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AvailabilityStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__1788CCACEAA9FFB0", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    UnitID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyID = table.Column<int>(type: "int", nullable: false),
                    UnitNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnitType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sizesqm = table.Column<double>(type: "float", nullable: true),
                    MonthlyRent = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    AvailabilityStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Available"),
                    ImgPath = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Unit__44F5EC9566254066", x => x.UnitID);
                    table.ForeignKey(
                        name: "FK_Unit_Property",
                        column: x => x.PropertyID,
                        principalTable: "Property",
                        principalColumn: "PropertyID");
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LogLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Source = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Log__5E5499A88C9281C8", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_Log_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Unread"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__20CF2E32E20FD78C", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_Notification_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    UnitID = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__6A4BEDF61FA2CC73", x => x.FeedbackID);
                    table.ForeignKey(
                        name: "FK_Feedback_Unit",
                        column: x => x.UnitID,
                        principalTable: "Unit",
                        principalColumn: "UnitID");
                    table.ForeignKey(
                        name: "FK_Feedback_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "LeaseApplication",
                columns: table => new
                {
                    ApplicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    UnitID = table.Column<int>(type: "int", nullable: false),
                    RequestedStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RequestedEndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LeaseApp__C93A4F79BC1A5CB8", x => x.ApplicationID);
                    table.ForeignKey(
                        name: "FK_LeaseApplication_Unit",
                        column: x => x.UnitID,
                        principalTable: "Unit",
                        principalColumn: "UnitID");
                    table.ForeignKey(
                        name: "FK_LeaseApplication_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRequest",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitID = table.Column<int>(type: "int", nullable: false),
                    TenantUserID = table.Column<int>(type: "int", nullable: false),
                    AssignedStaffID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Medium"),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    TicketNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ChangedByUserID = table.Column<int>(type: "int", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ResolvedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Maintena__33A8519AC78BB497", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequest_Staff",
                        column: x => x.AssignedStaffID,
                        principalTable: "User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_MaintenanceRequest_Status",
                        column: x => x.StatusID,
                        principalTable: "MaintenanceRequestStatus",
                        principalColumn: "StatusID");
                    table.ForeignKey(
                        name: "FK_MaintenanceRequest_Tenant",
                        column: x => x.TenantUserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_MaintenanceRequest_Unit",
                        column: x => x.UnitID,
                        principalTable: "Unit",
                        principalColumn: "UnitID");
                });

            migrationBuilder.CreateTable(
                name: "UnitAmenities",
                columns: table => new
                {
                    UnitAmenityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitID = table.Column<int>(type: "int", nullable: false),
                    AmenityID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UnitAmen__3F6BBFB16E562622", x => x.UnitAmenityID);
                    table.ForeignKey(
                        name: "FK_UnitAmenities_Amenities",
                        column: x => x.AmenityID,
                        principalTable: "Amenities",
                        principalColumn: "AmenityID");
                    table.ForeignKey(
                        name: "FK_UnitAmenities_Unit",
                        column: x => x.UnitID,
                        principalTable: "Unit",
                        principalColumn: "UnitID");
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    DocumentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    ApplicationID = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StoragePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Document__1ABEEF6FDC70F0D5", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_Document_Application",
                        column: x => x.ApplicationID,
                        principalTable: "LeaseApplication",
                        principalColumn: "ApplicationID");
                    table.ForeignKey(
                        name: "FK_Document_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Lease",
                columns: table => new
                {
                    LeaseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationID = table.Column<int>(type: "int", nullable: false),
                    LeaseStartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LeaseEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    MonthlyRent = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SecurityDeposit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ParentLeaseID = table.Column<int>(type: "int", nullable: true),
                    TerminationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TerminationDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lease__21FA58E1C76373B6", x => x.LeaseID);
                    table.ForeignKey(
                        name: "FK_Lease_LeaseApplication",
                        column: x => x.ApplicationID,
                        principalTable: "LeaseApplication",
                        principalColumn: "ApplicationID");
                    table.ForeignKey(
                        name: "FK_Lease_ParentLease",
                        column: x => x.ParentLeaseID,
                        principalTable: "Lease",
                        principalColumn: "LeaseID");
                });

            migrationBuilder.CreateTable(
                name: "LeaseApplicationStatusHistory",
                columns: table => new
                {
                    ApplicationStatusHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ChangedByUserID = table.Column<int>(type: "int", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LeaseApp__828D91DEE4081288", x => x.ApplicationStatusHistoryID);
                    table.ForeignKey(
                        name: "FK_AppStatusHistory_LeaseApplication",
                        column: x => x.ApplicationID,
                        principalTable: "LeaseApplication",
                        principalColumn: "ApplicationID");
                    table.ForeignKey(
                        name: "FK_AppStatusHistory_Status",
                        column: x => x.StatusID,
                        principalTable: "LeaseApplicationStatus",
                        principalColumn: "StatusID");
                    table.ForeignKey(
                        name: "FK_AppStatusHistory_User",
                        column: x => x.ChangedByUserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceStatusHistory",
                columns: table => new
                {
                    HistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    OldStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NewStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ChangedByUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Maintena__4D7B4ADD26D66E04", x => x.HistoryID);
                    table.ForeignKey(
                        name: "FK_StatusHistory_MaintenanceRequest",
                        column: x => x.RequestID,
                        principalTable: "MaintenanceRequest",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "LeaseStatusHistory",
                columns: table => new
                {
                    LeaseStatusHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ChangedByUserID = table.Column<int>(type: "int", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LeaseSta__5DF4886218D2F8BF", x => x.LeaseStatusHistoryID);
                    table.ForeignKey(
                        name: "FK_LeaseStatusHistory_Lease",
                        column: x => x.LeaseID,
                        principalTable: "Lease",
                        principalColumn: "LeaseID");
                    table.ForeignKey(
                        name: "FK_LeaseStatusHistory_Status",
                        column: x => x.StatusID,
                        principalTable: "LeaseStatus",
                        principalColumn: "StatusID");
                    table.ForeignKey(
                        name: "FK_LeaseStatusHistory_User",
                        column: x => x.ChangedByUserID,
                        principalTable: "User",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "PaymentRecord",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseID = table.Column<int>(type: "int", nullable: false),
                    AmountDue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    Notes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentR__9B556A5899B11BFD", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_PaymentRecord_Lease",
                        column: x => x.LeaseID,
                        principalTable: "Lease",
                        principalColumn: "LeaseID");
                });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "AmenityID", "AmenityName", "CreatedAt", "Description", "Icon", "IsActive" },
                values: new object[,]
                {
                    { 1, "Swimming Pool", new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9755), "Outdoor swimming pool with lounge area", "pool-icon", true },
                    { 2, "Gym", new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9757), "24/7 fitness center with modern equipment", "gym-icon", true },
                    { 3, "Parking", new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9759), "Secure underground parking", "parking-icon", true },
                    { 4, "Concierge", new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9761), "24-hour concierge service", "concierge-icon", true },
                    { 5, "Rooftop Garden", new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9762), "Landscaped rooftop garden with city views", "garden-icon", true },
                    { 6, "Sauna", new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9763), "Traditional Finnish sauna", "sauna-icon", true },
                    { 7, "Pet Friendly", new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9765), "Pet-friendly building with pet spa", "pet-icon", true },
                    { 8, "Smart Home", new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9766), "Smart home automation system", "smart-icon", true },
                    { 9, "Laundry Room", new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9769), "On-site laundry facilities", "laundry-icon", true },
                    { 10, "Business Center", new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9770), "Co-working space and meeting rooms", "business-icon", true }
                });

            migrationBuilder.InsertData(
                table: "LeaseApplicationStatus",
                columns: new[] { "StatusID", "CreatedAt", "Description", "DisplayOrder", "IsActive", "IsFinal", "StatusName" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9686), "Awaiting review", 1, true, false, "Pending" },
                    { 2, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9689), "Application under review", 2, true, false, "Screening" },
                    { 3, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9691), "Application approved", 3, true, false, "Approved" },
                    { 4, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9692), "Application rejected", 4, true, true, "Rejected" }
                });

            migrationBuilder.InsertData(
                table: "LeaseStatus",
                columns: new[] { "StatusID", "CreatedAt", "Description", "DisplayOrder", "IsActive", "IsTerminal", "StatusName" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9723), "Lease is active", 1, true, false, "Active" },
                    { 2, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9725), "Lease has expired", 2, true, true, "Expired" },
                    { 3, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9727), "Lease terminated early", 3, true, true, "Terminated" },
                    { 4, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9729), "Lease has been renewed", 4, true, false, "Renewed" }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceRequestStatus",
                columns: new[] { "StatusID", "CreatedAt", "Description", "DisplayOrder", "IsActive", "StatusName" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9434), "Request has been submitted", 1, true, "Submitted" },
                    { 2, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9451), "Assigned to staff", 2, true, "Assigned" },
                    { 3, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9453), "Work in progress", 3, true, "InProgress" },
                    { 4, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9455), "Issue resolved", 4, true, "Resolved" },
                    { 5, new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9456), "Request closed", 5, true, "Closed" }
                });

            migrationBuilder.InsertData(
                table: "Property",
                columns: new[] { "PropertyID", "Address", "City", "Description", "ImgPath", "Name", "PropertyType" },
                values: new object[,]
                {
                    { 1, "Building 123, Road 456", "Manama", "Luxury residential tower", "/images/pearl-tower.jpg", "The Pearl Tower", "Residential" },
                    { 2, "Seef District", "Manama", "Modern apartments with sea views", "/images/seef-views.jpg", "Seef Views", "Residential" },
                    { 3, "Amwaj Islands", "Muharraq", "Waterfront living", "/images/amwaj-plaza.jpg", "Amwaj Plaza", "Residential" },
                    { 4, "Juffair", "Manama", "Central location, great amenities", "/images/juffair-square.jpg", "Juffair Square", "Residential" },
                    { 5, "Diplomatic Area", "Manama", "Premium diplomatic area", "/images/diplomatic-heights.jpg", "Diplomatic Heights", "Commercial" },
                    { 6, "Bahrain Bay", "Manama", "Iconic waterfront property", "/images/bahrain-bay.jpg", "Bahrain Bay Tower", "Residential" },
                    { 7, "East Riffa", "Riffa", "Family-friendly community", "/images/riffa-views.jpg", "Riffa Views", "Residential" },
                    { 8, "Saar", "Saar", "Suburban living", "/images/saar-plaza.jpg", "Saar Plaza", "Residential" },
                    { 9, "Hamala", "Hamala", "Mixed-use development", "/images/al-liwan.jpg", "Al Liwan Village", "Commercial" },
                    { 10, "Diyar Al Muharraq", "Muharraq", "Beachfront community", "/images/marassi.jpg", "Marassi Al Bahrain", "Residential" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserID", "AvailabilityStatus", "Email", "FullName", "IdentityUserId", "Phone", "Role", "SkillProfile" },
                values: new object[,]
                {
                    { 1, null, "john.smith@example.com", "John Smith", null, "+97312345678", "Tenant", null },
                    { 2, null, "sarah.j@example.com", "Sarah Johnson", null, "+97312345679", "Tenant", null },
                    { 3, null, "mike.w@example.com", "Mike Wilson", null, "+97312345680", "Tenant", null },
                    { 4, null, "emma.b@example.com", "Emma Brown", null, "+97312345681", "Tenant", null },
                    { 5, "Available", "david.lee@example.com", "David Lee", null, "+97312345682", "PropertyManager", "Property Management" },
                    { 6, "Available", "lisa.c@example.com", "Lisa Chen", null, "+97312345683", "MaintenanceStaff", "Plumbing, Electrical" },
                    { 7, "Available", "robert.t@example.com", "Robert Taylor", null, "+97312345684", "MaintenanceStaff", "HVAC, General Repair" },
                    { 8, "Available", "maria.g@example.com", "Maria Garcia", null, "+97312345685", "PropertyManager", "Leasing, Customer Service" },
                    { 9, null, "james.w@example.com", "James Wilson", null, "+97312345686", "Tenant", null },
                    { 10, "Busy", "patricia.m@example.com", "Patricia Moore", null, "+97312345687", "MaintenanceStaff", "Carpentry, Painting" }
                });

            migrationBuilder.InsertData(
                table: "Log",
                columns: new[] { "LogID", "Action", "CreatedAt", "Details", "LogLevel", "Source", "UserID" },
                values: new object[,]
                {
                    { 1, "Login", new DateTime(2026, 4, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(498), "User logged in successfully", "Info", "Web", 5 },
                    { 2, "Create Lease", new DateTime(2026, 4, 14, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(500), "Created lease for Unit 101", "Info", "API", 5 },
                    { 3, "Update Request", new DateTime(2026, 4, 15, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(502), "Updated maintenance request status", "Info", "Web", 6 },
                    { 4, "Login Failed", new DateTime(2026, 4, 16, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(504), "Failed login attempt", "Warning", "Web", 7 },
                    { 5, "Generate Report", new DateTime(2026, 4, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(506), "Generated occupancy report", "Info", "API", 8 },
                    { 6, "Submit Application", new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(508), "Submitted lease application", "Info", "Web", 1 },
                    { 7, "Login", new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(510), "User logged in", "Info", "Web", 2 },
                    { 8, "Payment", new DateTime(2026, 4, 12, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(512), "Processed rent payment", "Info", "API", 3 },
                    { 9, "View Property", new DateTime(2026, 4, 14, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(514), "Viewed property details", "Info", "Web", 4 },
                    { 10, "Logout", new DateTime(2026, 4, 16, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(516), "User logged out", "Info", "Web", 9 }
                });

            migrationBuilder.InsertData(
                table: "Notification",
                columns: new[] { "NotificationID", "CreatedAt", "Message", "NotificationType", "Status", "UserID" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 29, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(403), "Your lease application has been approved!", "LeaseUpdate", "Read", 1 },
                    { 2, new DateTime(2026, 4, 15, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(405), "Maintenance request #REQ-002 has been assigned", "MaintenanceUpdate", "Read", 2 },
                    { 3, new DateTime(2026, 4, 16, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(407), "Rent payment due in 5 days", "PaymentReminder", "Unread", 3 },
                    { 4, new DateTime(2026, 4, 11, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(410), "Your unit inspection is scheduled for next week", "General", "Read", 4 },
                    { 5, new DateTime(2026, 4, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(411), "New lease application received for Pearl Tower Unit 101", "LeaseApplication", "Read", 5 },
                    { 6, new DateTime(2026, 4, 11, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(413), "You have been assigned to maintenance request #REQ-003", "MaintenanceUpdate", "Unread", 6 },
                    { 7, new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(415), "Maintenance request #REQ-004 has been resolved", "MaintenanceUpdate", "Read", 7 },
                    { 8, new DateTime(2026, 3, 19, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(416), "Welcome to Property Leasing System!", "General", "Read", 8 },
                    { 9, new DateTime(2026, 4, 15, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(418), "Your rent payment is now overdue", "PaymentReminder", "Unread", 9 },
                    { 10, new DateTime(2026, 4, 12, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(420), "New maintenance request submitted for your property", "General", "Read", 10 }
                });

            migrationBuilder.InsertData(
                table: "Unit",
                columns: new[] { "UnitID", "AvailabilityStatus", "ImgPath", "MonthlyRent", "PropertyID", "Sizesqm", "UnitNumber", "UnitType" },
                values: new object[,]
                {
                    { 1, "Available", "/images/unit101.jpg", 550.00m, 1, 85.5, "101", "Apartment" },
                    { 2, "Occupied", "/images/unit102.jpg", 650.00m, 1, 95.0, "102", "Apartment" },
                    { 3, "Available", "/images/unit201.jpg", 400.00m, 2, 45.0, "201", "Studio" },
                    { 4, "Available", "/images/unit202.jpg", 800.00m, 2, 110.0, "202", "Apartment" },
                    { 5, "Occupied", "/images/unit301.jpg", 950.00m, 3, 120.0, "301", "Apartment" },
                    { 6, "Available", "/images/unit302.jpg", 1800.00m, 3, 200.0, "302", "Penthouse" },
                    { 7, "Available", "/images/unit401.jpg", 350.00m, 4, 40.0, "401", "Studio" },
                    { 8, "Available", "/images/unit501.jpg", 1200.00m, 5, 150.0, "501", "Office" },
                    { 9, "Occupied", "/images/unit601.jpg", 1100.00m, 6, 130.0, "601", "Apartment" },
                    { 10, "Available", "/images/unit701.jpg", 1400.00m, 7, 180.0, "701", "Townhouse" }
                });

            migrationBuilder.InsertData(
                table: "Feedback",
                columns: new[] { "FeedbackID", "Comment", "CreatedAt", "IsVisible", "Rating", "UnitID", "UserID" },
                values: new object[,]
                {
                    { 1, "Great apartment, very clean and well-maintained!", new DateTime(2026, 4, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(450), true, 5, 1, 1 },
                    { 2, "Good value for money. AC works well.", new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(452), true, 4, 3, 2 },
                    { 3, "Decent place but parking is tight", new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(454), true, 3, 4, 3 },
                    { 4, "Excellent location and responsive management", new DateTime(2026, 3, 29, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(456), true, 5, 2, 4 },
                    { 5, "Professional office space, good amenities", new DateTime(2026, 4, 6, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(457), true, 4, 5, 5 },
                    { 6, "Had maintenance issues that took too long", new DateTime(2026, 4, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(459), true, 2, 6, 9 },
                    { 7, "Quiet neighborhood, friendly neighbors", new DateTime(2026, 3, 31, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(461), true, 4, 7, 1 },
                    { 8, "Perfect for business, would recommend", new DateTime(2026, 4, 4, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(464), true, 5, 8, 2 },
                    { 9, "Good but internet connection issues", new DateTime(2026, 4, 11, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(466), true, 3, 9, 3 },
                    { 10, "Spacious and modern design", new DateTime(2026, 4, 9, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(468), true, 4, 10, 4 }
                });

            migrationBuilder.InsertData(
                table: "LeaseApplication",
                columns: new[] { "ApplicationID", "CreatedAt", "Notes", "RequestedEndDate", "RequestedStartDate", "Status", "UnitID", "UserID" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(63), "First-time renter, interested in 1-year lease", new DateTime(2027, 5, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(58), new DateTime(2026, 5, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(49), "Approved", 1, 1 },
                    { 2, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(67), "Short-term lease requested", new DateTime(2026, 11, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(66), new DateTime(2026, 5, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(65), "Pending", 3, 2 },
                    { 3, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(71), "Family with 2 kids, need parking", new DateTime(2027, 6, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(70), new DateTime(2026, 6, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(69), "Screening", 4, 3 },
                    { 4, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(76), "Professional couple", new DateTime(2027, 5, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(75), new DateTime(2026, 5, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(74), "Approved", 2, 4 },
                    { 5, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(81), "Long-term lease preferred", new DateTime(2028, 6, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(78), new DateTime(2026, 6, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(77), "Pending", 5, 5 },
                    { 6, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(84), "Immediate move-in", new DateTime(2027, 4, 28, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 4, 28, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(82), "Approved", 6, 9 },
                    { 7, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(90), "Looking for quiet location", new DateTime(2027, 7, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(87), new DateTime(2026, 7, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(85), "Rejected", 7, 1 },
                    { 8, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(94), "Business professional", new DateTime(2027, 2, 12, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(93), new DateTime(2026, 5, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(91), "Screening", 8, 2 },
                    { 9, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(98), "Working from home", new DateTime(2027, 5, 23, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(97), new DateTime(2026, 5, 23, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(96), "Pending", 9, 3 },
                    { 10, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(102), "Need parking included", new DateTime(2028, 6, 7, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(100), new DateTime(2026, 6, 7, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(100), "Approved", 10, 4 }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceRequest",
                columns: new[] { "RequestID", "AssignedStaffID", "ChangedByUserID", "Description", "Priority", "RequestType", "ResolutionNotes", "ResolvedAt", "StatusID", "SubmittedAt", "TenantUserID", "TicketNumber", "Title", "UnitID" },
                values: new object[,]
                {
                    { 1, null, null, "Air conditioning unit making strange noises and not cooling", "High", "HVAC", null, null, 1, new DateTime(2026, 4, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(207), 1, "REQ-001", "AC not working", 1 },
                    { 2, 7, null, "Kitchen faucet leaking constantly", "Medium", "Plumbing", null, null, 2, new DateTime(2026, 4, 15, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(213), 2, "REQ-002", "Leaking faucet", 2 },
                    { 3, 6, null, "Light switch not working in bedroom", "Medium", "Electrical", null, null, 3, new DateTime(2026, 4, 11, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(216), 3, "REQ-003", "Electrical issue", 3 },
                    { 4, 7, null, "Shower drain clogged and slow", "Low", "Plumbing", "Drain cleared successfully", new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(220), 4, new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(219), 4, "REQ-004", "Clogged drain", 4 },
                    { 5, null, null, "Wall paint peeling in living room", "Low", "Maintenance", null, null, 1, new DateTime(2026, 4, 16, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(223), 5, "REQ-005", "Painting needed", 5 },
                    { 6, 10, null, "Living room window cracked", "High", "Repair", null, null, 2, new DateTime(2026, 4, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(225), 9, "REQ-006", "Window broken", 6 },
                    { 7, 6, null, "Front door lock sticking", "Medium", "Security", null, null, 3, new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(228), 1, "REQ-007", "Door lock issue", 7 },
                    { 8, null, null, "Ants in kitchen area", "Medium", "Cleaning", null, null, 1, new DateTime(2026, 4, 14, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(230), 2, "REQ-008", "Pest control", 8 },
                    { 9, 10, null, "No hot water", "High", "Plumbing", "Heating element replaced", new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(233), 4, new DateTime(2026, 4, 6, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(233), 3, "REQ-009", "Water heater", 9 },
                    { 10, 7, null, "Carpet stained and needs professional cleaning", "Low", "Cleaning", null, null, 1, new DateTime(2026, 4, 12, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(236), 4, "REQ-010", "Carpet cleaning", 10 }
                });

            migrationBuilder.InsertData(
                table: "UnitAmenities",
                columns: new[] { "UnitAmenityID", "AmenityID", "CreatedAt", "IsActive", "UnitID" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(364), true, 1 },
                    { 2, 2, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(365), true, 1 },
                    { 3, 3, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(366), true, 1 },
                    { 4, 1, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(368), true, 2 },
                    { 5, 2, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(369), true, 3 },
                    { 6, 1, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(370), true, 4 },
                    { 7, 7, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(371), true, 4 },
                    { 8, 1, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(372), true, 5 },
                    { 9, 2, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(374), true, 5 },
                    { 10, 8, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(375), true, 5 }
                });

            migrationBuilder.InsertData(
                table: "Lease",
                columns: new[] { "LeaseID", "ApplicationID", "CreatedAt", "LeaseEndDate", "LeaseStartDate", "MonthlyRent", "ParentLeaseID", "SecurityDeposit", "TerminationDate", "TerminationReason" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(138), new DateTime(2027, 5, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(135), new DateTime(2026, 5, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(135), 550.00m, null, 550.00m, null, null },
                    { 2, 4, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(144), new DateTime(2027, 5, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(143), new DateTime(2026, 5, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(142), 650.00m, null, 650.00m, null, null },
                    { 3, 6, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(148), new DateTime(2027, 4, 28, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(147), new DateTime(2026, 4, 28, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(146), 1100.00m, null, 1100.00m, null, null },
                    { 4, 10, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(152), new DateTime(2028, 6, 7, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(150), new DateTime(2026, 6, 7, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(150), 1400.00m, null, 1400.00m, null, null },
                    { 5, 2, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(155), new DateTime(2026, 11, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(154), new DateTime(2026, 5, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(154), 400.00m, null, 400.00m, null, null },
                    { 6, 5, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(159), new DateTime(2028, 6, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(158), new DateTime(2026, 6, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(157), 1200.00m, null, 1200.00m, null, null },
                    { 7, 9, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(163), new DateTime(2027, 5, 23, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(162), new DateTime(2026, 5, 23, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(161), 800.00m, null, 800.00m, null, null },
                    { 8, 3, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(168), new DateTime(2027, 6, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(166), new DateTime(2026, 6, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(166), 950.00m, null, 950.00m, null, null },
                    { 9, 8, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(172), new DateTime(2027, 2, 12, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(171), new DateTime(2026, 5, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(170), 350.00m, null, 350.00m, null, null },
                    { 10, 7, new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(176), new DateTime(2027, 7, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(174), new DateTime(2026, 7, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(174), 1800.00m, null, 1800.00m, null, null }
                });

            migrationBuilder.InsertData(
                table: "PaymentRecord",
                columns: new[] { "PaymentID", "AmountDue", "AmountPaid", "DueDate", "LeaseID", "Notes", "PaidDate", "PaymentStatus" },
                values: new object[,]
                {
                    { 1, 550.00m, 550.00m, new DateTime(2026, 4, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(304), 1, "January rent", new DateTime(2026, 4, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(305), "Paid" },
                    { 2, 550.00m, null, new DateTime(2026, 5, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(311), 1, "February rent", null, "Pending" },
                    { 3, 650.00m, 650.00m, new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(314), 2, "January rent", new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(315), "Paid" },
                    { 4, 1100.00m, 1100.00m, new DateTime(2026, 4, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(317), 3, "January rent", new DateTime(2026, 4, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(318), "Paid" },
                    { 5, 1100.00m, 500.00m, new DateTime(2026, 5, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(320), 3, "Partial payment received", null, "Partial" },
                    { 6, 1400.00m, null, new DateTime(2026, 4, 15, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(322), 4, "Late payment", null, "Overdue" },
                    { 7, 400.00m, 400.00m, new DateTime(2026, 3, 29, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(325), 5, "January rent", new DateTime(2026, 3, 29, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(326), "Paid" },
                    { 8, 1200.00m, null, new DateTime(2026, 4, 28, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(328), 6, "First month rent", null, "Pending" },
                    { 9, 800.00m, 800.00m, new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(331), 7, "January rent", new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(331), "Paid" },
                    { 10, 950.00m, null, new DateTime(2026, 5, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(333), 8, "Upcoming payment", null, "Pending" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Document_ApplicationID",
                table: "Document",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_Document_UserID",
                table: "Document",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UnitID",
                table: "Feedback",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserID",
                table: "Feedback",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Lease_ApplicationID",
                table: "Lease",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_Lease_ParentLeaseID",
                table: "Lease",
                column: "ParentLeaseID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseApplication_UnitID",
                table: "LeaseApplication",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseApplication_UserID",
                table: "LeaseApplication",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseApplicationStatusHistory_ChangedByUserID",
                table: "LeaseApplicationStatusHistory",
                column: "ChangedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseAppStatusHistory_ApplicationID",
                table: "LeaseApplicationStatusHistory",
                column: "ApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseAppStatusHistory_IsCurrent",
                table: "LeaseApplicationStatusHistory",
                column: "IsCurrent",
                filter: "([IsCurrent]=(1))");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseAppStatusHistory_StatusID",
                table: "LeaseApplicationStatusHistory",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseStatusHistory_ChangedByUserID",
                table: "LeaseStatusHistory",
                column: "ChangedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseStatusHistory_IsCurrent",
                table: "LeaseStatusHistory",
                column: "IsCurrent",
                filter: "([IsCurrent]=(1))");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseStatusHistory_LeaseID",
                table: "LeaseStatusHistory",
                column: "LeaseID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseStatusHistory_StatusID",
                table: "LeaseStatusHistory",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Log_UserID",
                table: "Log",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_AssignedStaffID",
                table: "MaintenanceRequest",
                column: "AssignedStaffID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_StatusID",
                table: "MaintenanceRequest",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_TenantUserID",
                table: "MaintenanceRequest",
                column: "TenantUserID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequest_UnitID",
                table: "MaintenanceRequest",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "UQ__Maintena__CBED06DA6A7B92C9",
                table: "MaintenanceRequest",
                column: "TicketNumber",
                unique: true,
                filter: "[TicketNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceStatusHistory_RequestID",
                table: "MaintenanceStatusHistory",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserID",
                table: "Notification",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRecord_LeaseID",
                table: "PaymentRecord",
                column: "LeaseID");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_PropertyID",
                table: "Unit",
                column: "PropertyID");

            migrationBuilder.CreateIndex(
                name: "IX_UnitAmenities_AmenityID",
                table: "UnitAmenities",
                column: "AmenityID");

            migrationBuilder.CreateIndex(
                name: "IX_UnitAmenities_UnitID",
                table: "UnitAmenities",
                column: "UnitID");

            migrationBuilder.CreateIndex(
                name: "UQ__User__A9D105342EFAB69E",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "LeaseApplicationStatusHistory");

            migrationBuilder.DropTable(
                name: "LeaseStatusHistory");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "MaintenanceStatusHistory");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "PaymentRecord");

            migrationBuilder.DropTable(
                name: "UnitAmenities");

            migrationBuilder.DropTable(
                name: "LeaseApplicationStatus");

            migrationBuilder.DropTable(
                name: "LeaseStatus");

            migrationBuilder.DropTable(
                name: "MaintenanceRequest");

            migrationBuilder.DropTable(
                name: "Lease");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "MaintenanceRequestStatus");

            migrationBuilder.DropTable(
                name: "LeaseApplication");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Property");
        }
    }
}
