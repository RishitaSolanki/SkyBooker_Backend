
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyBooker.FlightService.Models;

[Table("Airlines")]
public class Airline
{
    [Key]
    public int AirlineId { get; set; }

    [Required]
    [StringLength(100)]
    public string AirlineName { get; set; } = string.Empty;

    [Required]
    [StringLength(3)]
    public string IATACode { get; set; } = string.Empty;

    [StringLength(4)]
    public string? ICAOCode { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }

    [StringLength(500)]
    public string? LogoUrl { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation property
    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}