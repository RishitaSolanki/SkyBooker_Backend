using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkyBooker.FlightService.Migrations
{
    /// <inheritdoc />
    public partial class InitialFlightServiceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    AirlineId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AirlineName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IATACode = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ICAOCode = table.Column<string>(type: "TEXT", maxLength: 4, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    LogoUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.AirlineId);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    AirportId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IATACode = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ICAOCode = table.Column<string>(type: "TEXT", maxLength: 4, nullable: true),
                    AirportName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Timezone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.AirportId);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightNumber = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    AirlineId = table.Column<int>(type: "INTEGER", nullable: false),
                    OriginAirportCode = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    DestinationAirportCode = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    AircraftType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TotalSeats = table.Column<int>(type: "INTEGER", nullable: false),
                    AvailableSeats = table.Column<int>(type: "INTEGER", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightId);
                    table.ForeignKey(
                        name: "FK_Flights_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "AirlineId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Airlines",
                columns: new[] { "AirlineId", "AirlineName", "Country", "IATACode", "ICAOCode", "IsActive", "LogoUrl" },
                values: new object[,]
                {
                    { 1, "Air India", "India", "AI", "AIC", true, null },
                    { 2, "IndiGo", "India", "6E", "IGO", true, null },
                    { 3, "SpiceJet", "India", "SG", "SEJ", true, null },
                    { 4, "Vistara", "India", "UK", "VTI", true, null },
                    { 5, "AirAsia India", "India", "I5", "IAD", true, null }
                });

            migrationBuilder.InsertData(
                table: "Airports",
                columns: new[] { "AirportId", "AirportName", "City", "Country", "IATACode", "ICAOCode", "Latitude", "Longitude", "Timezone" },
                values: new object[,]
                {
                    { 1, "Indira Gandhi International Airport", "New Delhi", "India", "DEL", "VIDP", 28.5665m, 77.1031m, "Asia/Kolkata" },
                    { 2, "Chhatrapati Shivaji Maharaj International Airport", "Mumbai", "India", "BOM", "VABB", 19.0896m, 72.8656m, "Asia/Kolkata" },
                    { 3, "Kempegowda International Airport", "Bangalore", "India", "BLR", "VOBL", 13.1986m, 77.7066m, "Asia/Kolkata" },
                    { 4, "Chennai International Airport", "Chennai", "India", "MAA", "VOMM", 12.9941m, 80.1709m, "Asia/Kolkata" },
                    { 5, "Netaji Subhas Chandra Bose International Airport", "Kolkata", "India", "CCU", "VECC", 22.6547m, 88.4467m, "Asia/Kolkata" },
                    { 6, "Rajiv Gandhi International Airport", "Hyderabad", "India", "HYD", "VOHS", 17.2403m, 78.4294m, "Asia/Kolkata" },
                    { 7, "Pune Airport", "Pune", "India", "PNQ", "VAPO", 18.5822m, 73.9197m, "Asia/Kolkata" },
                    { 8, "Sardar Vallabhbhai Patel International Airport", "Ahmedabad", "India", "AMD", "VAAH", 23.0772m, 72.6347m, "Asia/Kolkata" }
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "FlightId", "AircraftType", "AirlineId", "ArrivalTime", "AvailableSeats", "BasePrice", "CreatedAt", "DepartureTime", "DestinationAirportCode", "DurationMinutes", "FlightNumber", "OriginAirportCode", "TotalSeats", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Boeing 787", 1, new DateTime(2025, 5, 1, 8, 30, 0, 0, DateTimeKind.Utc), 180, 5500.00m, new DateTime(2025, 4, 23, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 1, 6, 0, 0, 0, DateTimeKind.Utc), "BOM", 150, "AI101", "DEL", 250, null },
                    { 2, "Airbus A320", 2, new DateTime(2025, 5, 1, 11, 45, 0, 0, DateTimeKind.Utc), 120, 4200.00m, new DateTime(2025, 4, 23, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 5, 1, 10, 0, 0, 0, DateTimeKind.Utc), "BLR", 105, "6E202", "BOM", 180, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airline_IATACode",
                table: "Airlines",
                column: "IATACode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airport_IATACode",
                table: "Airports",
                column: "IATACode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flight_AirlineId",
                table: "Flights",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_FlightNumber",
                table: "Flights",
                column: "FlightNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Route_DateTime",
                table: "Flights",
                columns: new[] { "OriginAirportCode", "DestinationAirportCode", "DepartureTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Status",
                table: "Flights",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Airlines");
        }
    }
}
