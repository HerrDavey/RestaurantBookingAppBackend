using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Data;
using RestaurantBookingApp.Shared.Models;

namespace RestaurantBookingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return await _context.Reservations
                .Include(r => r.Restaurant)
                .ToListAsync();
        }

        // POST: api/reservations
        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            // Prosta walidacja dostępności przed zapisem
            var restaurant = await _context.Restaurants.FindAsync(reservation.RestaurantId);
            if (restaurant == null || restaurant.IsClosed)
            {
                return BadRequest("Restaurant not found or is closed.");
            }

            var reservedSeats = await _context.Reservations
                .Where(r => r.RestaurantId == reservation.RestaurantId
                         && r.ReservationDateTime == reservation.ReservationDateTime
                         && r.Status == "Confirmed")
                .SumAsync(r => r.SeatsReserved);

            if (restaurant.TotalSeats < reservedSeats + reservation.SeatsReserved)
            {
                return BadRequest("Not enough available seats for the selected time.");
            }

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }

        // Dodajmy metodę Get by Id dla CreatedAtAction
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservationById(int id)
        {
            var reservation = await _context.Reservations.Include(r => r.Restaurant).FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return reservation;
        }

        // PUT: api/reservations/{id}/cancel
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            reservation.Status = "Cancelled";
            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/reservations/mine?email=xxx
        [HttpGet("mine")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetMyReservations([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email address is required.");
            }

            return await _context.Reservations
                .Include(r => r.Restaurant)
                .Where(r => r.ClientEmail.ToLower() == email.ToLower())
                .OrderByDescending(r => r.ReservationDateTime)
                .ToListAsync();
        }

        // GET: api/reservations/filter?restaurantId=1&date=2025-06-07&tableNumber=5
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<object>>> FilterReservations(
            [FromQuery] int restaurantId,
            [FromQuery] DateTime date,
            [FromQuery] int? tableNumber)
        {
            var query = _context.Reservations
                .Include(r => r.Restaurant)
                .Where(r => r.RestaurantId == restaurantId && r.ReservationDateTime.Date == date.Date)
                .AsQueryable();

            if (tableNumber.HasValue)
            {
                query = query.Where(r => r.TableNumber == tableNumber.Value);
            }

            var reservations = await query
                .OrderBy(r => r.ReservationDateTime)
                .Select(r => new
                {
                    r.Id,
                    r.Restaurant.Name,
                    r.ReservationDateTime,
                    r.SeatsReserved,
                    r.ClientName,
                    r.ClientEmail,
                    r.Status,
                    r.TableNumber
                })
                .ToListAsync();

            return Ok(reservations);
        }
    }
}