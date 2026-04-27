using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyBooker.FlightService.Models;

[Table("Airports")]
public class Airport
{
    [Key]
    public int AirportId { get; set; }

    [Required]
    [StringLength(3)]
    public string IATACode { get; set; } = string.Empty;

    [StringLength(4)]
    public string? ICAOCode { get; set; }

    [Required]
    [StringLength(200)]
    public string AirportName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Country { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Timezone { get; set; }

    [Column(TypeName = "decimal(9,6)")]
    public decimal? Latitude { get; set; }

    [Column(TypeName = "decimal(9,6)")]
    public decimal? Longitude { get; set; }
}