namespace SkyBooker.SeatService.DTOs;

public class SeatDto
{
    public int SeatId { get; set; }
    public int FlightId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string SeatClass { get; set; } = string.Empty;
    public int Row { get; set; }
    public int Column { get; set; }
    public bool IsWindow { get; set; }
    public bool IsAisle { get; set; }
    public bool HasExtraLegroom { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal PriceMultiplier { get; set; }
    public DateTime? HeldSince { get; set; }
    public DateTime? ConfirmedAt { get; set; }
}
