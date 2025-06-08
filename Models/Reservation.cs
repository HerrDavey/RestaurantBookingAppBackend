namespace RestaurantBookingApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public int SeatsReserved { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string Status { get; set; } = "Confirmed"; // "Confirmed", "Cancelled"
        public int? TableNumber { get; set; }
    }
}
