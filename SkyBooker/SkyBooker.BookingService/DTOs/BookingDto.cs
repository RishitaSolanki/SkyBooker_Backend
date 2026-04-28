namespace SkyBooker.BookingService.DTOs;

public class BookingDto
{
    public string BookingId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int FlightId { get; set; }
    public string PnrCode { get; set; } = string.Empty;
    public string TripType { get; set; } = "ONE_WAY";
    public string Status { get; set; } = "PENDING";
    public decimal BaseFare { get; set; }
    public decimal Taxes { get; set; }
    public decimal TotalFare { get; set; }
    public string MealPreference { get; set; } = "Veg";
    public int LuggageKg { get; set; }
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public DateTime BookedAt { get; set; }
    public string? PaymentId { get; set; }
}
