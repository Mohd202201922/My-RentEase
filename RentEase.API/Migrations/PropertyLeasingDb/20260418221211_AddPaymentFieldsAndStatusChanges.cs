using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RentEase.API.Migrations.PropertyLeasingDb
{
    /// <inheritdoc />
    public partial class AddPaymentFieldsAndStatusChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First delete payment records that reference leases 5-10
            migrationBuilder.DeleteData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValues: new object[] { 7, 8, 9, 10 });   // Adjust IDs as needed

            migrationBuilder.DeleteData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequest",
                keyColumn: "RequestID",
                keyValue: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "LeaseApplication",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaymentApproved",
                table: "LeaseApplication",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentAmount",
                table: "LeaseApplication",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentApprovedAt",
                table: "LeaseApplication",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "LeaseApplication",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTransactionId",
                table: "LeaseApplication",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8978));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8980));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8981));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8983));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8985));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8986));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8987));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8988));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8989));

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "AmenityID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9030));

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 1,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Great apartment!", new DateTime(2026, 4, 4, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9540) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Good value", new DateTime(2026, 4, 9, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9543) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 3,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Parking is tight", new DateTime(2026, 4, 11, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9544) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 4,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Excellent location", new DateTime(2026, 3, 30, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9546) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 5,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Professional space", new DateTime(2026, 4, 7, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9548) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 6,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Maintenance slow", new DateTime(2026, 4, 14, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9551) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 7,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Quiet neighborhood", new DateTime(2026, 4, 1, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9552) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 8,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Perfect for business", new DateTime(2026, 4, 5, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9554) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 9,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Internet issues", new DateTime(2026, 4, 12, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9556) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 10,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Spacious design", new DateTime(2026, 4, 10, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9557) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9372), new DateTime(2027, 5, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9370), new DateTime(2026, 5, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9369) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9376), new DateTime(2027, 5, 9, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9374), new DateTime(2026, 5, 9, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9374) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9381), new DateTime(2027, 4, 29, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9378), new DateTime(2026, 4, 29, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9377) });

            migrationBuilder.UpdateData(
                table: "Lease",
                keyColumn: "LeaseID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "LeaseEndDate", "LeaseStartDate" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9384), new DateTime(2028, 6, 8, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9383), new DateTime(2026, 6, 8, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9382) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IsPaymentApproved", "Notes", "PaymentAmount", "PaymentApprovedAt", "PaymentDate", "PaymentTransactionId", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9254), true, "First-time renter", null, null, new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9255), null, new DateTime(2027, 5, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9248), new DateTime(2026, 5, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9235) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Notes", "PaymentAmount", "PaymentApprovedAt", "PaymentDate", "PaymentTransactionId", "RequestedEndDate", "RequestedStartDate", "Status" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9263), "Short-term lease", null, null, null, null, new DateTime(2026, 11, 3, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9260), new DateTime(2026, 5, 4, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9260), "Screening" });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Notes", "PaymentAmount", "PaymentApprovedAt", "PaymentDate", "PaymentTransactionId", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9267), "Family with kids", null, null, null, null, new DateTime(2027, 6, 3, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9266), new DateTime(2026, 6, 3, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9265) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "IsPaymentApproved", "PaymentAmount", "PaymentApprovedAt", "PaymentDate", "PaymentTransactionId", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9271), true, null, null, new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9271), null, new DateTime(2027, 5, 9, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9270), new DateTime(2026, 5, 9, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9269) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Notes", "PaymentAmount", "PaymentApprovedAt", "PaymentDate", "PaymentTransactionId", "RequestedEndDate", "RequestedStartDate", "Status" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9276), "Long-term lease", null, null, null, null, new DateTime(2028, 6, 18, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9274), new DateTime(2026, 6, 18, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9273), "Screening" });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 6,
                columns: new[] { "CreatedAt", "IsPaymentApproved", "PaymentAmount", "PaymentApprovedAt", "PaymentDate", "PaymentTransactionId", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9279), true, null, null, new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9280), null, new DateTime(2027, 4, 29, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9278), new DateTime(2026, 4, 29, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9277) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Notes", "PaymentAmount", "PaymentApprovedAt", "PaymentDate", "PaymentTransactionId", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9283), "Quiet location", null, null, null, null, new DateTime(2027, 7, 18, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9282), new DateTime(2026, 7, 18, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9281) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 8,
                columns: new[] { "CreatedAt", "PaymentAmount", "PaymentApprovedAt", "PaymentDate", "PaymentTransactionId", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9286), null, null, null, null, new DateTime(2027, 2, 13, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9285), new DateTime(2026, 5, 14, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9284) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 9,
                columns: new[] { "CreatedAt", "PaymentAmount", "PaymentApprovedAt", "PaymentDate", "PaymentTransactionId", "RequestedEndDate", "RequestedStartDate", "Status" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9289), null, null, null, null, new DateTime(2027, 5, 24, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9288), new DateTime(2026, 5, 24, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9288), "Screening" });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 10,
                columns: new[] { "CreatedAt", "IsPaymentApproved", "Notes", "PaymentAmount", "PaymentApprovedAt", "PaymentDate", "PaymentTransactionId", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9295), true, "Need parking", null, null, new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9296), null, new DateTime(2028, 6, 8, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9293), new DateTime(2026, 6, 8, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9291) });

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DisplayOrder" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8910), 1 });

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DisplayOrder" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8912), 2 });

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "DisplayOrder" },
                values: new object[] { new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8914), 3 });

            migrationBuilder.InsertData(
                table: "LeaseApplicationStatus",
                columns: new[] { "StatusID", "CreatedAt", "Description", "DisplayOrder", "IsActive", "IsFinal", "StatusName" },
                values: new object[,]
                {
                    { 5, new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8916), "Lease renewal requested", 4, true, false, "Renewal" },
                    { 6, new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8917), "Lease terminated", 5, true, true, "Terminated" }
                });

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8947));

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8949));

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8950));

            migrationBuilder.UpdateData(
                table: "LeaseStatus",
                keyColumn: "StatusID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8952));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Details" },
                values: new object[] { new DateTime(2026, 4, 14, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9584), "User logged in" });

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 15, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9586));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Details" },
                values: new object[] { new DateTime(2026, 4, 16, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9588), "Updated maintenance request" });

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 17, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9589));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 18, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9591));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 9, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9593));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 11, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9594));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 13, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9596));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 15, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9598));

            migrationBuilder.UpdateData(
                table: "Log",
                keyColumn: "LogID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 17, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9638));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8704));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8706));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8708));

            migrationBuilder.UpdateData(
                table: "MaintenanceRequestStatus",
                keyColumn: "StatusID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(8709));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 30, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9498));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 16, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9500));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 17, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9501));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 12, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9503));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 4, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9505));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 12, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9506));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 9, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9508));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9510));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 16, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9511));

            migrationBuilder.UpdateData(
                table: "Notification",
                keyColumn: "NotificationID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 13, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9513));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 1,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 14, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9409), new DateTime(2026, 4, 14, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9410) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 2,
                column: "DueDate",
                value: new DateTime(2026, 5, 14, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9413));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 3,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 9, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9416), new DateTime(2026, 4, 9, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9416) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 4,
                columns: new[] { "DueDate", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 4, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9418), new DateTime(2026, 4, 4, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9419) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 5,
                columns: new[] { "DueDate", "Notes" },
                values: new object[] { new DateTime(2026, 5, 4, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9421), "Partial payment" });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 6,
                column: "DueDate",
                value: new DateTime(2026, 4, 16, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9422));

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 7,
                columns: new[] { "DueDate", "LeaseID", "PaidDate" },
                values: new object[] { new DateTime(2026, 3, 30, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9424), 1, new DateTime(2026, 3, 30, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9424) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 8,
                columns: new[] { "DueDate", "LeaseID" },
                values: new object[] { new DateTime(2026, 4, 29, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9426), 2 });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 9,
                columns: new[] { "DueDate", "LeaseID", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 11, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9428), 3, new DateTime(2026, 4, 11, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9428) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 10,
                columns: new[] { "DueDate", "LeaseID" },
                values: new object[] { new DateTime(2026, 5, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9430), 4 });

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9462));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9463));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9464));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9465));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9466));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9467));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9468));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9469));

            migrationBuilder.UpdateData(
                table: "UnitAmenities",
                keyColumn: "UnitAmenityID",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 4, 19, 1, 12, 10, 506, DateTimeKind.Local).AddTicks(9470));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "IsPaymentApproved",
                table: "LeaseApplication");

            migrationBuilder.DropColumn(
                name: "PaymentAmount",
                table: "LeaseApplication");

            migrationBuilder.DropColumn(
                name: "PaymentApprovedAt",
                table: "LeaseApplication");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "LeaseApplication");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionId",
                table: "LeaseApplication");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "LeaseApplication",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

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
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Great apartment, very clean and well-maintained!", new DateTime(2026, 4, 3, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(714) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 2,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Good value for money. AC works well.", new DateTime(2026, 4, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(717) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 3,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Decent place but parking is tight", new DateTime(2026, 4, 10, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(720) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 4,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Excellent location and responsive management", new DateTime(2026, 3, 29, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(722) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 5,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Professional office space, good amenities", new DateTime(2026, 4, 6, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(724) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 6,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Had maintenance issues that took too long", new DateTime(2026, 4, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(726) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 7,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Quiet neighborhood, friendly neighbors", new DateTime(2026, 3, 31, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(729) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 8,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Perfect for business, would recommend", new DateTime(2026, 4, 4, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(731) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 9,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Good but internet connection issues", new DateTime(2026, 4, 11, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(733) });

            migrationBuilder.UpdateData(
                table: "Feedback",
                keyColumn: "FeedbackID",
                keyValue: 10,
                columns: new[] { "Comment", "CreatedAt" },
                values: new object[] { "Spacious and modern design", new DateTime(2026, 4, 9, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(736) });

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

            migrationBuilder.InsertData(
                table: "Lease",
                columns: new[] { "LeaseID", "ApplicationID", "CreatedAt", "LeaseEndDate", "LeaseStartDate", "MonthlyRent", "ParentLeaseID", "SecurityDeposit", "TerminationDate", "TerminationReason" },
                values: new object[,]
                {
                    { 5, 2, new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(363), new DateTime(2026, 11, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(360), new DateTime(2026, 5, 3, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(360), 400.00m, null, 400.00m, null, null },
                    { 6, 5, new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(367), new DateTime(2028, 6, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(365), new DateTime(2026, 6, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(365), 1200.00m, null, 1200.00m, null, null },
                    { 7, 9, new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(371), new DateTime(2027, 5, 23, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(369), new DateTime(2026, 5, 23, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(369), 800.00m, null, 800.00m, null, null },
                    { 8, 3, new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(375), new DateTime(2027, 6, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(374), new DateTime(2026, 6, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(373), 950.00m, null, 950.00m, null, null },
                    { 9, 8, new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(380), new DateTime(2027, 2, 12, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(378), new DateTime(2026, 5, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(378), 350.00m, null, 350.00m, null, null },
                    { 10, 7, new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(384), new DateTime(2027, 7, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(383), new DateTime(2026, 7, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(382), 1800.00m, null, 1800.00m, null, null }
                });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Notes", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(253), "First-time renter, interested in 1-year lease", new DateTime(2027, 5, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(248), new DateTime(2026, 5, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(240) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Notes", "RequestedEndDate", "RequestedStartDate", "Status" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(260), "Short-term lease requested", new DateTime(2026, 11, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(257), new DateTime(2026, 5, 3, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(256), "Pending" });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Notes", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(264), "Family with 2 kids, need parking", new DateTime(2027, 6, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(262), new DateTime(2026, 6, 2, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(262) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(268), new DateTime(2027, 5, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(267), new DateTime(2026, 5, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(266) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Notes", "RequestedEndDate", "RequestedStartDate", "Status" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(274), "Long-term lease preferred", new DateTime(2028, 6, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(272), new DateTime(2026, 6, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(270), "Pending" });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 6,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(278), new DateTime(2027, 4, 28, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(277), new DateTime(2026, 4, 28, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(276) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 7,
                columns: new[] { "CreatedAt", "Notes", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(282), "Looking for quiet location", new DateTime(2027, 7, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(281), new DateTime(2026, 7, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(280) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 8,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(288), new DateTime(2027, 2, 12, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(287), new DateTime(2026, 5, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(286) });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 9,
                columns: new[] { "CreatedAt", "RequestedEndDate", "RequestedStartDate", "Status" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(292), new DateTime(2027, 5, 23, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(291), new DateTime(2026, 5, 23, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(290), "Pending" });

            migrationBuilder.UpdateData(
                table: "LeaseApplication",
                keyColumn: "ApplicationID",
                keyValue: 10,
                columns: new[] { "CreatedAt", "Notes", "RequestedEndDate", "RequestedStartDate" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(296), "Need parking included", new DateTime(2028, 6, 7, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(295), new DateTime(2026, 6, 7, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(294) });

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DisplayOrder" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9819), 2 });

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DisplayOrder" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9821), 3 });

            migrationBuilder.UpdateData(
                table: "LeaseApplicationStatus",
                keyColumn: "StatusID",
                keyValue: 4,
                columns: new[] { "CreatedAt", "DisplayOrder" },
                values: new object[] { new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9824), 4 });

            migrationBuilder.InsertData(
                table: "LeaseApplicationStatus",
                columns: new[] { "StatusID", "CreatedAt", "Description", "DisplayOrder", "IsActive", "IsFinal", "StatusName" },
                values: new object[] { 1, new DateTime(2026, 4, 18, 16, 29, 43, 353, DateTimeKind.Local).AddTicks(9816), "Awaiting review", 1, true, false, "Pending" });

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
                columns: new[] { "CreatedAt", "Details" },
                values: new object[] { new DateTime(2026, 4, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(776), "User logged in successfully" });

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
                columns: new[] { "CreatedAt", "Details" },
                values: new object[] { new DateTime(2026, 4, 15, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(782), "Updated maintenance request status" });

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

            migrationBuilder.InsertData(
                table: "MaintenanceRequest",
                columns: new[] { "RequestID", "AssignedStaffID", "ChangedByUserID", "Description", "Priority", "RequestType", "ResolutionNotes", "ResolvedAt", "StatusID", "SubmittedAt", "TenantUserID", "TicketNumber", "Title", "UnitID" },
                values: new object[,]
                {
                    { 1, null, null, "Air conditioning unit making strange noises and not cooling", "High", "HVAC", null, null, 1, new DateTime(2026, 4, 13, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(471), 1, "REQ-001", "AC not working", 1 },
                    { 2, 7, null, "Kitchen faucet leaking constantly", "Medium", "Plumbing", null, null, 2, new DateTime(2026, 4, 15, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(477), 2, "REQ-002", "Leaking faucet", 2 },
                    { 3, 6, null, "Light switch not working in bedroom", "Medium", "Electrical", null, null, 3, new DateTime(2026, 4, 11, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(481), 3, "REQ-003", "Electrical issue", 3 },
                    { 4, 7, null, "Shower drain clogged and slow", "Low", "Plumbing", "Drain cleared successfully", new DateTime(2026, 4, 10, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(485), 4, new DateTime(2026, 4, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(484), 4, "REQ-004", "Clogged drain", 4 },
                    { 5, null, null, "Wall paint peeling in living room", "Low", "Maintenance", null, null, 1, new DateTime(2026, 4, 16, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(488), 5, "REQ-005", "Painting needed", 5 },
                    { 6, 10, null, "Living room window cracked", "High", "Repair", null, null, 2, new DateTime(2026, 4, 17, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(492), 9, "REQ-006", "Window broken", 6 },
                    { 7, 6, null, "Front door lock sticking", "Medium", "Security", null, null, 3, new DateTime(2026, 4, 10, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(495), 1, "REQ-007", "Door lock issue", 7 },
                    { 8, null, null, "Ants in kitchen area", "Medium", "Cleaning", null, null, 1, new DateTime(2026, 4, 14, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(498), 2, "REQ-008", "Pest control", 8 },
                    { 9, 10, null, "No hot water", "High", "Plumbing", "Heating element replaced", new DateTime(2026, 4, 8, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(502), 4, new DateTime(2026, 4, 6, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(501), 3, "REQ-009", "Water heater", 9 },
                    { 10, 7, null, "Carpet stained and needs professional cleaning", "Low", "Cleaning", null, null, 1, new DateTime(2026, 4, 12, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(506), 4, "REQ-010", "Carpet cleaning", 10 }
                });

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
                columns: new[] { "DueDate", "Notes" },
                values: new object[] { new DateTime(2026, 5, 3, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(560), "Partial payment received" });

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
                columns: new[] { "DueDate", "LeaseID", "PaidDate" },
                values: new object[] { new DateTime(2026, 3, 29, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(564), 5, new DateTime(2026, 3, 29, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(565) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 8,
                columns: new[] { "DueDate", "LeaseID" },
                values: new object[] { new DateTime(2026, 4, 28, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(567), 6 });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 9,
                columns: new[] { "DueDate", "LeaseID", "PaidDate" },
                values: new object[] { new DateTime(2026, 4, 10, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(569), 7, new DateTime(2026, 4, 10, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(570) });

            migrationBuilder.UpdateData(
                table: "PaymentRecord",
                keyColumn: "PaymentID",
                keyValue: 10,
                columns: new[] { "DueDate", "LeaseID" },
                values: new object[] { new DateTime(2026, 5, 18, 16, 29, 43, 354, DateTimeKind.Local).AddTicks(572), 8 });

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
        }
    }
}
