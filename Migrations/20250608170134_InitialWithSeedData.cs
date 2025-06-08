using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantBookingApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CuisineType = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    TotalSeats = table.Column<int>(type: "INTEGER", nullable: false),
                    AveragePrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsClosed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RestaurantId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReservationDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SeatsReserved = table.Column<int>(type: "INTEGER", nullable: false),
                    ClientName = table.Column<string>(type: "TEXT", nullable: false),
                    ClientEmail = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    TableNumber = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "Id", "AveragePrice", "CuisineType", "Description", "IsClosed", "Location", "Name", "TotalSeats" },
                values: new object[,]
                {
                    { 1, 85.00m, "Włoska", "Autentyczna włoska kuchnia w sercu Krakowa. Specjalizujemy się w pizzy neapolitańskiej i świeżych makaronach.", false, "Kraków, Rynek Główny 10", "Trattoria da Vinci", 50 },
                    { 2, 120.00m, "Japońska", "Nowoczesne sushi przygotowywane przez doświadczonych mistrzów. Świeże ryby dostarczane codziennie.", false, "Warszawa, ul. Złota 59", "Sushi Master", 30 },
                    { 3, 60.00m, "Polska", "Tradycyjne dania kuchni podhalańskiej w regionalnym wystroju. Spróbuj naszego oscypka z żurawiną!", false, "Zakopane, Krupówki 25", "Góralska Chata", 80 },
                    { 4, 150.00m, "Francuska", "Klimatyczne bistro z klasykami kuchni francuskiej. Idealne miejsce na romantyczną kolację.", true, "Gdańsk, ul. Długa 15", "Le Petit Paris", 25 }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "ClientEmail", "ClientName", "ReservationDateTime", "RestaurantId", "SeatsReserved", "Status", "TableNumber" },
                values: new object[,]
                {
                    { 1, "jan.kowalski@example.com", "Jan Kowalski", new DateTime(2025, 6, 11, 19, 0, 0, 0, DateTimeKind.Local), 1, 2, "Confirmed", 5 },
                    { 2, "anna.nowak@example.com", "Anna Nowak", new DateTime(2025, 6, 13, 20, 0, 0, 0, DateTimeKind.Local), 2, 4, "Confirmed", 1 },
                    { 3, "piotr.zielinski@example.com", "Piotr Zieliński", new DateTime(2025, 6, 11, 19, 0, 0, 0, DateTimeKind.Local), 1, 3, "Confirmed", 6 },
                    { 4, "k.wisniewska@example.com", "Katarzyna Wiśniewska", new DateTime(2025, 6, 7, 18, 0, 0, 0, DateTimeKind.Local), 3, 6, "Confirmed", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RestaurantId",
                table: "Reservations",
                column: "RestaurantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Restaurants");
        }
    }
}
