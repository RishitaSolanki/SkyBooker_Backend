using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkyBooker.FlightService.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgresCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    AirlineId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AirlineName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IATACode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    ICAOCode = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.AirlineId);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    AirportId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IATACode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    ICAOCode = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    AirportName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Timezone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(9,6)", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric(9,6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.AirportId);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FlightNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AirlineId = table.Column<int>(type: "integer", nullable: false),
                    OriginAirportCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    DestinationAirportCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    AircraftType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TotalSeats = table.Column<int>(type: "integer", nullable: false),
                    AvailableSeats = table.Column<int>(type: "integer", nullable: false),
                    BusinessPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    EconomyPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                columns: new[] { "FlightId", "AircraftType", "AirlineId", "ArrivalTime", "AvailableSeats", "BusinessPrice", "CreatedAt", "DepartureTime", "DestinationAirportCode", "DurationMinutes", "EconomyPrice", "FlightNumber", "OriginAirportCode", "TotalSeats", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Boeing 787", 1, new DateTime(2026, 5, 14, 8, 30, 0, 0, DateTimeKind.Unspecified), 180, 8500.00m, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 14, 6, 0, 0, 0, DateTimeKind.Unspecified), "BOM", 150, 5500.00m, "AI101", "DEL", 250, null },
                    { 2, "Airbus A320", 2, new DateTime(2026, 5, 14, 11, 45, 0, 0, DateTimeKind.Unspecified), 120, 6200.00m, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 14, 10, 0, 0, 0, DateTimeKind.Unspecified), "BLR", 105, 4200.00m, "6E202", "BOM", 180, null },
                    { 3, "Airbus A321", 4, new DateTime(2026, 5, 14, 20, 30, 0, 0, DateTimeKind.Unspecified), 45, 9200.00m, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), "BOM", 150, 6200.00m, "UK303", "DEL", 200, null },
                    { 4, "Boeing 737", 3, new DateTime(2026, 5, 15, 10, 0, 0, 0, DateTimeKind.Unspecified), 90, 7200.00m, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 15, 7, 30, 0, 0, DateTimeKind.Unspecified), "BOM", 150, 4800.00m, "SG404", "DEL", 180, null },
                    { 5, "Boeing 787", 1, new DateTime(2026, 5, 14, 23, 30, 0, 0, DateTimeKind.Unspecified), 200, 8200.00m, new DateTime(2026, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 14, 21, 0, 0, 0, DateTimeKind.Unspecified), "DEL", 150, 5200.00m, "AI102", "BOM", 250, null }
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
