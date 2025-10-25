using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrafficSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "Darnytskyi" },
                    { 5, "Holosiivskyi" },
                    { 6, "Solomianskyi" },
                    { 7, "Dniprovskiy" },
                    { 8, "Obolonskyi" },
                    { 9, "Sviatoshynskyi" },
                    { 10, "Desnianskyi" }
                });

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2025, 1, 1, 12, 5, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 3,
                column: "Timestamp",
                value: new DateTime(2025, 1, 1, 12, 10, 0, 0, DateTimeKind.Utc));

            migrationBuilder.InsertData(
                table: "TrafficData",
                columns: new[] { "Id", "AreaId", "RoadName", "Timestamp", "TrafficLevel" },
                values: new object[,]
                {
                    { 4, 4, "Darnytskyi", new DateTime(2025, 1, 1, 12, 15, 0, 0, DateTimeKind.Utc), "Medium" },
                    { 5, 5, "Holosiivskyi", new DateTime(2025, 1, 1, 12, 20, 0, 0, DateTimeKind.Utc), "Comfortable" },
                    { 6, 6, "Solomianskyi", new DateTime(2025, 1, 1, 12, 25, 0, 0, DateTimeKind.Utc), "Moderate" },
                    { 7, 7, "Dniprovskiy", new DateTime(2025, 1, 1, 12, 30, 0, 0, DateTimeKind.Utc), "High" },
                    { 8, 8, "Obolonskyi", new DateTime(2025, 1, 1, 12, 35, 0, 0, DateTimeKind.Utc), "Medium" },
                    { 9, 9, "Sviatoshynskyi", new DateTime(2025, 1, 1, 12, 40, 0, 0, DateTimeKind.Utc), "Comfortable" },
                    { 10, 10, "Desnianskyi", new DateTime(2025, 1, 1, 12, 45, 0, 0, DateTimeKind.Utc), "Low" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrafficData_AreaId",
                table: "TrafficData",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrafficData_Areas_AreaId",
                table: "TrafficData",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrafficData_Areas_AreaId",
                table: "TrafficData");

            migrationBuilder.DropIndex(
                name: "IX_TrafficData_AreaId",
                table: "TrafficData");

            migrationBuilder.DeleteData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 2,
                column: "Timestamp",
                value: new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 3,
                column: "Timestamp",
                value: new DateTime(2025, 1, 1, 12, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
