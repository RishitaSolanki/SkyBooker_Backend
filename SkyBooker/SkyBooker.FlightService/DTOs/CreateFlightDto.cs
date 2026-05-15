using System.ComponentModel.DataAnnotations;

namespace SkyBooker.FlightService.DTOs;

public class CreateFlightDto
{
    [Required]
    [StringLength(10)]
    public string FlightNumber { get; set; } = string.Empty;

    [Required]
    public int AirlineId { get; set; }

    [Required]
    [StringLength(3)]
    public string OriginAirportCode { get; set; } = string.Empty;

    [Required]
    [StringLength(3)]
    public string DestinationAirportCode { get; set; } = string.Empty;

    [Required]
    public DateTime DepartureTime { get; set; }

    [Required]
    public DateTime ArrivalTime { get; set; }

    [Required]
    [StringLength(50)]
    public string AircraftType { get; set; } = string.Empty;

    [Required]
    [Range(1, 1000)]
    public int TotalSeats { get; set; }

    [Required]
    [Range(0, 100000)]
    public decimal BusinessPrice { get; set; }
    public decimal EconomyPrice { get; set; }
}