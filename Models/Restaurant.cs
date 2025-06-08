namespace RestaurantBookingApp.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CuisineType { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int TotalSeats { get; set; }
        public decimal AveragePrice { get; set; }   
        public bool IsClosed { get; set; } = false;
    }
}
