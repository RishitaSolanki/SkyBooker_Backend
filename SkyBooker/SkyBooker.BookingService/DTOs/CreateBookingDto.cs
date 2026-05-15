namespace SkyBooker.BookingService.DTOs;

public class CreateBookingDto
{
    public string UserId { get; set; } = string.Empty;
    public int FlightId { get; set; }
    public string TripType { get; set; } = "ONE_WAY";
    public string MealPreference { get; set; } = "Veg";
    public int LuggageKg { get; set; } = 15;
    public decimal BaseFare { get; set; }
    public decimal Taxes { get; set; }
    public decimal TotalFare { get; set; }
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
}
