using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyBooker.FlightService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "FlightId",
                keyValue: 1,
                columns: new[] { "ArrivalTime", "CreatedAt", "DepartureTime" },
                values: new object[] { new DateTime(2026, 5, 4, 8, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 4, 6, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "FlightId",
                keyValue: 2,
                columns: new[] { "ArrivalTime", "CreatedAt", "DepartureTime" },
                values: new object[] { new DateTime(2026, 5, 4, 11, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 4, 10, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "FlightId",
                keyValue: 1,
                columns: new[] { "ArrivalTime", "CreatedAt", "DepartureTime" },
                values: new object[] { new DateTime(2025, 5, 1, 8, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 23, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 1, 6, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "FlightId",
                keyValue: 2,
                columns: new[] { "ArrivalTime", "CreatedAt", "DepartureTime" },
                values: new object[] { new DateTime(2025, 5, 1, 11, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 23, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 1, 10, 0, 0, 0, DateTimeKind.Utc) });
        }
    }
}
