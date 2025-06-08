using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Data;
using RestaurantBookingApp.Models;

namespace RestaurantBookingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController: ControllerBase
    {
        private readonly AppDbContext _context;

        public RestaurantsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/restaurants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            return await _context.Restaurants.ToListAsync();
        }

        // GET: api/restaurants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return restaurant;
        }

        // POST: api/restaurants
        [HttpPost]
        public async Task<ActionResult<Restaurant>> PostRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRestaurant), new { id = restaurant.Id }, restaurant);
        }

        // PUT: api/restaurants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurant(int id, Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return BadRequest();
            }

            _context.Entry(restaurant).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/restaurants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/restaurants/{id}/availability
        [HttpGet("{id}/availability")]
        public async Task<ActionResult<object>> GetAvailability(int id, [FromQuery] DateTime dateTime)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            // Liczba miejsc zajętych na dany dzień/godzinę
            var reservedSeats = await _context.Reservations
                .Where(r => r.RestaurantId == id
                         && r.ReservationDateTime == dateTime
                         && r.Status == "Confirmed")
                .SumAsync(r => r.SeatsReserved);

            var availableSeats = restaurant.TotalSeats - reservedSeats;

            return Ok(new
            {
                RestaurantId = id,
                DateTime = dateTime,
                TotalSeats = restaurant.TotalSeats,
                ReservedSeats = reservedSeats,
                AvailableSeats = availableSeats
            });
        }
    }
}

