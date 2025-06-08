// Plik: RestaurantBookingApp/Controllers/RestaurantsController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Data;
using RestaurantBookingApp.Shared.Models;

namespace RestaurantBookingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : ControllerBase
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

        // GET: api/restaurants/search?cuisine=...&location=...&minSeats=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Restaurant>>> SearchRestaurants(
            [FromQuery] string? cuisine,
            [FromQuery] string? location,
            [FromQuery] int? minSeats,
            [FromQuery] decimal? maxPrice)
        {
            var query = _context.Restaurants.AsQueryable();

            if (!string.IsNullOrEmpty(cuisine))
            {
                query = query.Where(r => r.CuisineType.ToLower().Contains(cuisine.ToLower()));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(r => r.Location.ToLower().Contains(location.ToLower()));
            }

            if (minSeats.HasValue && minSeats > 0)
            {
                // To jest uproszczenie. Pełna implementacja wymagałaby sprawdzania dostępności
                // w czasie rzeczywistym, ale na potrzeby filtrowania listy jest OK.
                query = query.Where(r => r.TotalSeats >= minSeats);
            }

            if (maxPrice.HasValue && maxPrice > 0)
            {
                query = query.Where(r => r.AveragePrice <= maxPrice);
            }

            return await query.Where(r => !r.IsClosed).ToListAsync();
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Restaurants.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

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

        
        [HttpGet("{id}/availability")]
        public async Task<ActionResult<object>> GetAvailability(int id, [FromQuery] string date, [FromQuery] string time)
            {
            // Walidacja, czy otrzymaliśmy poprawne dane
            if (!DateOnly.TryParse(date, out var parsedDate) || !TimeOnly.TryParse(time, out var parsedTime))
            {
                return BadRequest("Invalid date or time format. Please use YYYY-MM-DD and HH:MM.");
            }

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            if (restaurant.IsClosed)
            {
                return Ok(new
                {
                    IsAvailable = false,
                    Message = "Restaurant is closed."
                });
            }

            // Łączymy datę i godzinę w jeden obiekt DateTime
            var dateTime = parsedDate.ToDateTime(parsedTime);

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
                AvailableSeats = availableSeats,
                IsAvailable = availableSeats > 0
            });
        }
    }
}