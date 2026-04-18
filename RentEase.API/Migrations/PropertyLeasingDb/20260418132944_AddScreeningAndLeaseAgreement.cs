using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentEase.API.Migrations.PropertyLeasingDb
{
    /// <inheritdoc />
    public partial class AddScreeningAndLeaseAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "LeaseApplication",
                type: "datetime",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScreeningAppointment",
                columns: table => new
                {
                    ScreeningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ManagerNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreeningAppointment", x => x.ScreeningId);
                    table.ForeignKey(
                        name: "FK_ScreeningAppointment_LeaseApplication_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "LeaseApplication",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScreeningAppointment_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "UnitID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScreeningAppointment_User_TenantId",
                        column: x => x.TenantId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaseAgreement",
                columns: table => new
                {
                    LeaseAgreementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    ScreeningId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    LeaseStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaseEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthlyRent = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SecurityDeposit = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    LateFeePerDay = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Draft"),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SpecialClauses = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SignedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseAgreement", x => x.LeaseAgreementId);
                    table.ForeignKey(
                        name: "FK_LeaseAgreement_LeaseApplication_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "LeaseApplication",
                        principalColumn: "ApplicationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaseAgreement_ScreeningAppointment_ScreeningId",
                        column: x => x.ScreeningId,
                        principalTable: "ScreeningAppointment",
                        principalColumn: "ScreeningId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaseAgreement_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "UnitID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaseAgreement_User_TenantId",
                        column: x => x.TenantId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9898));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9900));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9902));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9904));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9905));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9907));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9909));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9910));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9912));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 3, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(714));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(717));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 10, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(720));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(722));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(724));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(726));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(729));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 4, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(731));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 11, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(733));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 9, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(736));

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(343), new DateTime(2027, 5, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(338), new DateTime(2026, 5, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(336) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(349), new DateTime(2027, 5, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(348), new DateTime(2026, 5, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(347) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(354), new DateTime(2027, 4, 28, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(352), new DateTime(2026, 4, 28, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(352) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(358), new DateTime(2028, 6, 7, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(356), new DateTime(2026, 6, 7, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(356) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 5,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(363), new DateTime(2026, 11, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(360), new DateTime(2026, 5, 3, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(360) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 6,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(367), new DateTime(2028, 6, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(365), new DateTime(2026, 6, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(365) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 7,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(371), new DateTime(2027, 5, 23, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(369), new DateTime(2026, 5, 23, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(369) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 8,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(375), new DateTime(2027, 6, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(374), new DateTime(2026, 6, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(373) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 9,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(380), new DateTime(2027, 2, 12, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(378), new DateTime(2026, 5, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(378) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 10,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(384), new DateTime(2027, 7, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(383), new DateTime(2026, 7, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(382) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(253), new DateTime(2027, 5, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(248), new DateTime(2026, 5, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(240), null });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(260), new DateTime(2026, 11, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(257), new DateTime(2026, 5, 3, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(256), null });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(264), new DateTime(2027, 6, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(262), new DateTime(2026, 6, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(262), null });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(268), new DateTime(2027, 5, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(267), new DateTime(2026, 5, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(266), null });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 5,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(274), new DateTime(2028, 6, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(272), new DateTime(2026, 6, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(270), null });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 6,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(278), new DateTime(2027, 4, 28, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(277), new DateTime(2026, 4, 28, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(276), null });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 7,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(282), new DateTime(2027, 7, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(281), new DateTime(2026, 7, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(280), null });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 8,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(288), new DateTime(2027, 2, 12, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(287), new DateTime(2026, 5, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(286), null });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 9,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(292), new DateTime(2027, 5, 23, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(291), new DateTime(2026, 5, 23, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(290), null });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 10,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(296), new DateTime(2028, 6, 7, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(295), new DateTime(2026, 6, 7, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(294), null });

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9821));

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9854));

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9859));

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9861));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(776));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 14, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(779));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 15, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(782));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 16, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(784));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(786));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(789));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 10, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(791));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 12, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(793));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 14, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(795));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 16, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(798));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 1,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(471));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 2,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 15, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(477));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 3,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 11, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(481));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 4,
                columns: new[] { "ResolvedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(485), new DateTime(2026, 4, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(484) });

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 5,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 16, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(488));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 6,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(492));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 7,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 10, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(495));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 8,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 14, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(498));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 9,
                columns: new[] { "ResolvedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(502), new DateTime(2026, 4, 6, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(501) });

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 10,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 12, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(506));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9442));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9462));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9464));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9465));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(660));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 15, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(663));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 16, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(665));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 11, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(667));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 3, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(669));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 11, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(672));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(674));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 19, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(676));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 15, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(678));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 12, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(680));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 1,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(546), new DateTime(2026, 4, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(547) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 2,
                column: "DueDate",
                value: new DateTime(2026, 5, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(550));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 3,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(554), new DateTime(2026, 4, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(554) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 4,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 3, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(557), new DateTime(2026, 4, 3, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(557) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 5,
                column: "DueDate",
                value: new DateTime(2026, 5, 3, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(560));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 6,
                column: "DueDate",
                value: new DateTime(2026, 4, 15, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(562));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 7,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 3, 29, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(564), new DateTime(2026, 3, 29, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(565) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 8,
                column: "DueDate",
                value: new DateTime(2026, 4, 28, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(567));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 9,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(569), new DateTime(2026, 4, 10, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(570) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 10,
                column: "DueDate",
                value: new DateTime(2026, 5, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(572));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(614));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(616));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(617));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(619));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(620));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(621));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(623));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(624));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(625));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(626));

            migrationBuilder.CreateIndex(
                name: "IX_LeaseAgreement_ApplicationId",
                table: "LeaseAgreement",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseAgreement_ScreeningId",
                table: "LeaseAgreement",
                column: "ScreeningId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaseAgreement_TenantId",
                table: "LeaseAgreement",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseAgreement_UnitId",
                table: "LeaseAgreement",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningAppointment_ApplicationId",
                table: "ScreeningAppointment",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningAppointment_TenantId",
                table: "ScreeningAppointment",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreeningAppointment_UnitId",
                table: "ScreeningAppointment",
                column: "UnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaseAgreement");

            migrationBuilder.DropTable(
                name: "ScreeningAppointment");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LeaseApplication");

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9755));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9757));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9759));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9761));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9762));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9763));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9765));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9766));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9769));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(450));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(452));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(454));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(456));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 6, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(457));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(459));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 31, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(461));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 4, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(464));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 11, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(466));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 9, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(468));

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(138), new DateTime(2027, 5, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(135), new DateTime(2026, 5, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(135) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(144), new DateTime(2027, 5, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(143), new DateTime(2026, 5, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(142) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(148), new DateTime(2027, 4, 28, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(147), new DateTime(2026, 4, 28, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(146) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(152), new DateTime(2028, 6, 7, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(150), new DateTime(2026, 6, 7, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(150) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 5,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(155), new DateTime(2026, 11, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(154), new DateTime(2026, 5, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(154) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 6,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(159), new DateTime(2028, 6, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(158), new DateTime(2026, 6, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(157) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 7,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(163), new DateTime(2027, 5, 23, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(162), new DateTime(2026, 5, 23, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(161) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 8,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(168), new DateTime(2027, 6, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(166), new DateTime(2026, 6, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(166) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 9,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(172), new DateTime(2027, 2, 12, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(171), new DateTime(2026, 5, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(170) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 10,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(176), new DateTime(2027, 7, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(174), new DateTime(2026, 7, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(174) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(63), new DateTime(2027, 5, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(58), new DateTime(2026, 5, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(49) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(67), new DateTime(2026, 11, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(66), new DateTime(2026, 5, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(65) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(71), new DateTime(2027, 6, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(70), new DateTime(2026, 6, 2, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(69) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(76), new DateTime(2027, 5, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(75), new DateTime(2026, 5, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(74) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 5,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(81), new DateTime(2028, 6, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(78), new DateTime(2026, 6, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(77) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 6,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(84), new DateTime(2027, 4, 28, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(83), new DateTime(2026, 4, 28, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(82) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 7,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(90), new DateTime(2027, 7, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(87), new DateTime(2026, 7, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(85) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 8,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(94), new DateTime(2027, 2, 12, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(93), new DateTime(2026, 5, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(91) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 9,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(98), new DateTime(2027, 5, 23, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(97), new DateTime(2026, 5, 23, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(96) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 10,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(102), new DateTime(2028, 6, 7, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(100), new DateTime(2026, 6, 7, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(100) });

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9686));

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9689));

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9691));

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9692));

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9723));

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(498));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 14, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(500));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 15, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(502));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 16, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(504));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(506));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(508));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(510));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 12, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(512));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 14, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(514));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 16, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(516));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 1,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(207));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 2,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 15, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 3,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 11, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(216));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 4,
                columns: new[] { "ResolvedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(220), new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(219) });

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 5,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 16, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(223));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 6,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 17, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(225));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 7,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(228));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 8,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 14, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(230));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 9,
                columns: new[] { "ResolvedAt", "SubmittedAt" },
                values: new object[] { new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(233), new DateTime(2026, 4, 6, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(233) });

            migrationBuilder.UpdateData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 10,
                column: "SubmittedAt",
                value: new DateTime(2026, 4, 12, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(236));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9434));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9451));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9453));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9455));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 101, DateTimeKind.Local).AddTicks(9456));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 29, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(403));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 15, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(405));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 16, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(407));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 11, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(410));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(411));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 11, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(415));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 19, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(416));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 15, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(418));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 12, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(420));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 1,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(304), new DateTime(2026, 4, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(305) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 2,
                column: "DueDate",
                value: new DateTime(2026, 5, 13, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(311));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 3,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(314), new DateTime(2026, 4, 8, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(315) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 4,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(317), new DateTime(2026, 4, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(318) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 5,
                column: "DueDate",
                value: new DateTime(2026, 5, 3, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(320));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 6,
                column: "DueDate",
                value: new DateTime(2026, 4, 15, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(322));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 7,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 3, 29, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(325), new DateTime(2026, 3, 29, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(326) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 8,
                column: "DueDate",
                value: new DateTime(2026, 4, 28, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(328));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 9,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(331), new DateTime(2026, 4, 10, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(331) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 10,
                column: "DueDate",
                value: new DateTime(2026, 5, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(333));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(364));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(365));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(366));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(368));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(369));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(370));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(371));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(372));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(374));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 14, 37, 19, 102, DateTimeKind.Local).AddTicks(375));
        }
    }
}
