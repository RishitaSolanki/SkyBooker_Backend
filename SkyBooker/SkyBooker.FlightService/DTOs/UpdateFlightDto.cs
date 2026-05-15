using System.ComponentModel.DataAnnotations;
using SkyBooker.FlightService.Common.Enums;

namespace SkyBooker.FlightService.DTOs;

public class UpdateFlightDto
{
    [StringLength(50)]
    public string? AircraftType { get; set; }

    public FlightStatus? Status { get; set; }

    [Range(0, 100000)]
    public decimal? BasePrice { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    [Range(0, 1000)]
    public int? AvailableSeats { get; set; }
}