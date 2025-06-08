using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Models;
using System.Collections.Generic;

namespace RestaurantBookingApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
