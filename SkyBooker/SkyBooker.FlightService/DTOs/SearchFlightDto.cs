using System.ComponentModel.DataAnnotations;

namespace SkyBooker.FlightService.DTOs;

public class SearchFlightDto
{
    [Required]
    [StringLength(3)]
    public string OriginAirportCode { get; set; } = string.Empty;

    [Required]
    [StringLength(3)]
    public string DestinationAirportCode { get; set; } = string.Empty;

    [Required]
    public DateTime DepartureDate { get; set; }

    [Range(1, 10)]
    public int Passengers { get; set; } = 1;

    public FlightFilterDto? Filters { get; set; }
}