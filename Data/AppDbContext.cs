using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Shared.Models;

namespace RestaurantBookingApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Pobieramy dzisiejszą datę, aby rezerwacje były zawsze "aktualne" względem dnia testów
            var today = DateTime.Now.Date;

            // === DANE STARTOWE DLA RESTAURACJI ===
            modelBuilder.Entity<Restaurant>().HasData(
                new Restaurant
                {
                    Id = 1,
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
                    IsClosed = false
                },
                new Restaurant
                {
                    Id = 5,
                    Name = "El Sombrero",
                    CuisineType = "Meksykańska",
                    Location = "Wrocław, Plac Solny 5",
                    Description = "Fiesta smaków prosto z Meksyku. Prawdziwe tacos, burrito i orzeźwiająca margarita.",
                    TotalSeats = 40,
                    AveragePrice = 75.00m,
                    IsClosed = false
                },
                new Restaurant
                {
                    Id = 6,
                    Name = "Steakhouse 'Angus'",
                    CuisineType = "Amerykańska",
                    Location = "Poznań, Stary Rynek 100",
                    Description = "Najlepsze steki w mieście. Sezonowana wołowina, idealnie wysmażona według Twoich preferencji.",
                    TotalSeats = 60,
                    AveragePrice = 180.00m,
                    IsClosed = true // Ta restauracja jest oznaczona jako zamknięta
                }
            );

            // === DANE STARTOWE DLA REZERWACJI (z uwzględnieniem 8 czerwca i okolic) ===
            modelBuilder.Entity<Reservation>().HasData(
                // --- Rezerwacje na 8 czerwca ---
                new Reservation
                {
                    Id = 1,
                    RestaurantId = 1, // Trattoria da Vinci
                    ReservationDateTime = new DateTime(today.Year, 6, 8, 18, 0, 0),
                    SeatsReserved = 4,
                    ClientName = "Alicja Testowa",
                    ClientEmail = "alicja.test@example.com",
                    Status = "Confirmed",
                    TableNumber = 1
                },
                new Reservation
                {
                    Id = 2,
                    RestaurantId = 1, // Trattoria da Vinci
                    ReservationDateTime = new DateTime(today.Year, 6, 8, 18, 0, 0), // Ten sam termin, inny stolik
                    SeatsReserved = 2,
                    ClientName = "Bartosz Programista",
                    ClientEmail = "b.programista@example.com",
                    Status = "Confirmed",
                    TableNumber = 2
                },
                new Reservation
                {
                    Id = 3,
                    RestaurantId = 2, // Sushi Master
                    ReservationDateTime = new DateTime(today.Year, 6, 8, 20, 30, 0),
                    SeatsReserved = 2,
                    ClientName = "Celina Wdrożenie",
                    ClientEmail = "celina.w@example.com",
                    Status = "Confirmed",
                    TableNumber = 5
                },

                // --- Rezerwacje na 9 czerwca ---
                new Reservation
                {
                    Id = 4,
                    RestaurantId = 5, // El Sombrero
                    ReservationDateTime = new DateTime(today.Year, 6, 9, 19, 0, 0),
                    SeatsReserved = 6,
                    ClientName = "Dawid Debug",
                    ClientEmail = "d.debug@example.com",
                    Status = "Confirmed",
                    TableNumber = 10
                },
                new Reservation
                {
                    Id = 5,
                    RestaurantId = 3, // Góralska Chata
                    ReservationDateTime = new DateTime(today.Year, 6, 9, 17, 0, 0),
                    SeatsReserved = 8,
                    ClientName = "Ewa Error",
                    ClientEmail = "ewa.error@example.com",
                    Status = "Cancelled" // Rezerwacja anulowana
                },

                // --- Rezerwacje na kolejne dni ---
                new Reservation
                {
                    Id = 6,
                    RestaurantId = 4, // Le Petit Paris
                    ReservationDateTime = new DateTime(today.Year, 6, 10, 20, 0, 0),
                    SeatsReserved = 2,
                    ClientName = "Fabian Frontend",
                    ClientEmail = "fabian.f@example.com",
                    Status = "Confirmed",
                    TableNumber = 3
                }
            );
        }
    }
}