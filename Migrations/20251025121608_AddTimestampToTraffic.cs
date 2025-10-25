using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _.Migrations
{
    /// <inheritdoc />
    public partial class AddTimestampToTraffic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "TrafficData",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RoadName", "Timestamp" },
                values: new object[] { "Shevchenkivskyi", new DateTime(2025, 10, 25, 12, 16, 7, 846, DateTimeKind.Utc).AddTicks(6490) });

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "RoadName", "Timestamp" },
                values: new object[] { "Pecherskyi", new DateTime(2025, 10, 25, 12, 16, 7, 846, DateTimeKind.Utc).AddTicks(6590) });

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "RoadName", "Timestamp" },
                values: new object[] { "Podilskyi", new DateTime(2025, 10, 25, 12, 16, 7, 846, DateTimeKind.Utc).AddTicks(6590) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "TrafficData");

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoadName",
                value: "");

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoadName",
                value: "");

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoadName",
                value: "");
        }
    }
}
