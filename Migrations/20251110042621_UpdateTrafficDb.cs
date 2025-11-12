using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrafficDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 4,
                column: "TrafficLevel",
                value: "Comfortable");

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 5,
                column: "TrafficLevel",
                value: "Low");

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 8,
                column: "TrafficLevel",
                value: "Low");

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 10,
                column: "TrafficLevel",
                value: "High");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 4,
                column: "TrafficLevel",
                value: "Medium");

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 5,
                column: "TrafficLevel",
                value: "Comfortable");

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 8,
                column: "TrafficLevel",
                value: "Medium");

            migrationBuilder.UpdateData(
                table: "TrafficData",
                keyColumn: "Id",
                keyValue: 10,
                column: "TrafficLevel",
                value: "Low");
        }
    }
}
