using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Shared.Models;

namespace RestaurantBookingApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        // Ta metoda jest kluczowa dla dodawania danych startowych
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === DANE STARTOWE DLA RESTAURACJI ===
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant
                {
                    Id = 1, // Ważne: musimy jawnie podać ID dla danych startowych
                    Name = "Trattoria da Vinci",
                    CuisineType = "Włoska",
                    Location = "Kraków, Rynek Główny 10",
                    Description = "Autentyczna włoska kuchnia w sercu Krakowa. Specjalizujemy się w pizzy neapolitańskiej i świeżych makaronach.",
                    TotalSeats = 50,
                    AveragePrice = 85.00m,
                    IsClosed = false
                },
                new Restaurant
                {
                    Id = 2,
                    Name = "Sushi Master",
                    CuisineType = "Japońska",
                    Location = "Warszawa, ul. Złota 59",
                    Description = "Nowoczesne sushi przygotowywane przez doświadczonych mistrzów. Świeże ryby dostarczane codziennie.",
                    TotalSeats = 30,
                    AveragePrice = 120.00m,
                    IsClosed = false
                },
                new Restaurant
                {
                    Id = 3,
                    Name = "Góralska Chata",
                    CuisineType = "Polska",
                    Location = "Zakopane, Krupówki 25",
                    Description = "Tradycyjne dania kuchni podhalańskiej w regionalnym wystroju. Spróbuj naszego oscypka z żurawiną!",
                    TotalSeats = 80,
                    AveragePrice = 60.00m,
                    IsClosed = false
                },
                new Restaurant
                {
                    Id = 4,
                    Name = "Le Petit Paris",
                    CuisineType = "Francuska",
                    Location = "Gdańsk, ul. Długa 15",
                    Description = "Klimatyczne bistro z klasykami kuchni francuskiej. Idealne miejsce na romantyczną kolację.",
                    TotalSeats = 25,
                    AveragePrice = 150.00m,
                    IsClosed = true // Ta restauracja będzie oznaczona jako zamknięta
                }
            );

            // === DANE STARTOWE DLA REZERWACJI ===
            modelBuilder.Entity<Reservation>().HasData(
                new Reservation
                {
                    Id = 1,
                    RestaurantId = 1, // Rezerwacja w "Trattoria da Vinci"
                    ReservationDateTime = DateTime.Now.Date.AddDays(3).AddHours(19), // Za 3 dni o 19:00
                    SeatsReserved = 2,
                    ClientName = "Jan Kowalski",
                    ClientEmail = "jan.kowalski@example.com",
                    Status = "Confirmed",
                    TableNumber = 5
                },
                new Reservation
                {
                    Id = 2,
                    RestaurantId = 2, // Rezerwacja w "Sushi Master"
                    ReservationDateTime = DateTime.Now.Date.AddDays(5).AddHours(20), // Za 5 dni o 20:00
                    SeatsReserved = 4,
                    ClientName = "Anna Nowak",
                    ClientEmail = "anna.nowak@example.com",
                    Status = "Confirmed",
                    TableNumber = 1
                },
                new Reservation
                {
                    Id = 3,
                    RestaurantId = 1, // Kolejna rezerwacja w "Trattoria da Vinci"
                    ReservationDateTime = DateTime.Now.Date.AddDays(3).AddHours(19), // Ten sam termin co rezerwacja 1
                    SeatsReserved = 3,
                    ClientName = "Piotr Zieliński",
                    ClientEmail = "piotr.zielinski@example.com",
                    Status = "Confirmed",
                    TableNumber = 6
                },
                new Reservation
                {
                    Id = 4,
                    RestaurantId = 3, // Rezerwacja w "Góralska Chata"
                    ReservationDateTime = DateTime.Now.Date.AddDays(-1).AddHours(18), // Rezerwacja z przeszłości
                    SeatsReserved = 6,
                    ClientName = "Katarzyna Wiśniewska",
                    ClientEmail = "k.wisniewska@example.com",
                    Status = "Confirmed"
                }
            );
        }
    }
}