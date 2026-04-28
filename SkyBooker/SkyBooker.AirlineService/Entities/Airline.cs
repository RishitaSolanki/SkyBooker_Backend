using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyBooker.AirlineService.Entities;

public class Airline
{
    [Key]
    public string AirlineId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(3)]
    public string IataCode { get; set; } = string.Empty;

    [StringLength(3)]
    public string IcaoCode { get; set; } = string.Empty;

    [StringLength(500)]
    public string LogoUrl { get; set; } = string.Empty;

    [StringLength(200)]
    public string Country { get; set; } = string.Empty;

    [StringLength(100)]
    public string ContactEmail { get; set; } = string.Empty;

    [StringLength(20)]
    public string ContactPhone { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; } = true;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
