using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SkyBooker.FlightService.Common.Enums;

namespace SkyBooker.FlightService.Models;

[Table("Flights")]
public class Flight
{
    [Key]
    public int FlightId { get; set; }

    [Required]
    [StringLength(10)]
    public string FlightNumber { get; set; } = string.Empty;

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

    public int DurationMinutes { get; set; }

    [Required]
    public FlightStatus Status { get; set; } = FlightStatus.Scheduled;

    [Required]
    [StringLength(50)]
    public string AircraftType { get; set; } = string.Empty;

    public int TotalSeats { get; set; }

    public int AvailableSeats { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal BasePrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    [ForeignKey(nameof(AirlineId))]
    public virtual Airline Airline { get; set; } = null!;
}