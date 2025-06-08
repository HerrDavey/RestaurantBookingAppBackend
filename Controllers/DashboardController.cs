using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Data;

namespace RestaurantBookingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/dashboard?restaurantId=1&date=2025-06-07
        [HttpGet]
        public async Task<ActionResult<object>> GetDashboard([FromQuery] int restaurantId, [FromQuery] DateTime date)
        {
            var restaurant = await _context.Restaurants.FindAsync(restaurantId);

            if (restaurant == null)
            {
                return NotFound();
            }

            // Wszystkie rezerwacje na dany dzień
            var reservations = await _context.Reservations
                .Where(r => r.RestaurantId == restaurantId
                         && r.ReservationDateTime.Date == date.Date
                         && r.Status == "Confirmed")
                .ToListAsync();

            var reservedSeats = reservations.Sum(r => r.SeatsReserved);
            var totalSeats = restaurant.TotalSeats;
            var availableSeats = totalSeats - reservedSeats;
            var occupancyPercent = totalSeats > 0 ? (double)reservedSeats / totalSeats * 100 : 0;

            return Ok(new
            {
                RestaurantId = restaurantId,
                Date = date.Date,
                TotalSeats = totalSeats,
                ReservedSeats = reservedSeats,
                AvailableSeats = availableSeats,
                OccupancyPercent = Math.Round(occupancyPercent, 2),
                TotalReservations = reservations.Count
            });
        }
    }
}