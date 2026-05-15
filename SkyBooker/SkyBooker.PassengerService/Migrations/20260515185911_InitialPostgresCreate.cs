using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyBooker.PassengerService.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgresCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PassengerInfos",
                columns: table => new
                {
                    PassengerId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    Title = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PassportNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nationality = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PassportExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SeatId = table.Column<string>(type: "text", nullable: true),
                    SeatNumber = table.Column<string>(type: "text", nullable: true),
                    TicketNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PassengerType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "ADULT"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerInfos", x => x.PassengerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PassengerInfos_BookingId",
                table: "PassengerInfos",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerInfos_PassportNumber",
                table: "PassengerInfos",
                column: "PassportNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerInfos_SeatId",
                table: "PassengerInfos",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_PassengerInfos_TicketNumber",
                table: "PassengerInfos",
                column: "TicketNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PassengerInfos");
        }
    }
}
