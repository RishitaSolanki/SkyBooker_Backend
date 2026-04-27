namespace SkyBooker.FlightService.DTOs;

public class FlightFilterDto
{
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? AirlineId { get; set; }
    public TimeSpan? EarliestDepartureTime { get; set; }
    public TimeSpan? LatestDepartureTime { get; set; }
    public TimeSpan? EarliestArrivalTime { get; set; }
    public TimeSpan? LatestArrivalTime { get; set; }
    public int? MaxStops { get; set; }
}