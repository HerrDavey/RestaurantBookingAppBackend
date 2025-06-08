using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantBookingApp.Data;
using RestaurantBookingApp.Models;

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
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReservations), new { id = reservation.Id }, reservation);
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
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/reservations/mine?email=xxx
        [HttpGet("mine")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetMyReservations([FromQuery] string email)
        {
            return await _context.Reservations
                .Include(r => r.Restaurant)
                .Where(r => r.ClientEmail == email)
                .OrderByDescending(r => r.ReservationDateTime)
                .ToListAsync();
        }


        // GET: api/reservations/filter?date=2025-06-07
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<object>>> FilterReservations([FromQuery] DateTime date)
        {
            var reservations = await _context.Reservations
                .Include(r => r.Restaurant)
                .Where(r => r.ReservationDateTime.Date == date.Date)
                .OrderBy(r => r.ReservationDateTime)
                .Select(r => new
                {
                    r.Id,
                    r.Restaurant.Name,
                    r.ReservationDateTime,
                    r.SeatsReserved,
                    r.ClientName,
                    r.Status
                })
                .ToListAsync();

            return Ok(reservations);
        }
    }
}
