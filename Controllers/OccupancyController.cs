using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Data;

namespace RestaurantBookingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OccupancyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OccupancyController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/occupancy?restaurantId=1&dateTime=2025-06-07T19:00:00
        [HttpGet]
        public async Task<ActionResult<object>> GetOccupancy([FromQuery] int restaurantId, [FromQuery] DateTime dateTime)
        {
            var restaurant = await _context.Restaurants.FindAsync(restaurantId);

            if (restaurant == null)
            {
                return NotFound();
            }

            // Liczba zajętych miejsc
            var reservedSeats = await _context.Reservations
                .Where(r => r.RestaurantId == restaurantId
                         && r.ReservationDateTime == dateTime
                         && r.Status == "Confirmed")
                .SumAsync(r => r.SeatsReserved);

            var totalSeats = restaurant.TotalSeats;
            var occupancyPercent = totalSeats > 0 ? (double)reservedSeats / totalSeats * 100 : 0;

            return Ok(new
            {
                RestaurantId = restaurantId,
                DateTime = dateTime,
                TotalSeats = totalSeats,
                ReservedSeats = reservedSeats,
                OccupancyPercent = Math.Round(occupancyPercent, 2)
            });
        }
    }
}
