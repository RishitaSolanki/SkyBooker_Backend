using System.ComponentModel.DataAnnotations;

namespace SkyBooker.SeatService.DTOs;

public class CreateSeatDto
{
    [Required]
    public int FlightId { get; set; }

    [Required]
    [MaxLength(10)]
    public string SeatNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string SeatClass { get; set; } = string.Empty;

    [Required]
    public int Row { get; set; }

    [Required]
    public int Column { get; set; }

    [Required]
    public bool IsWindow { get; set; }

    [Required]
    public bool IsAisle { get; set; }

    [Required]
    public bool HasExtraLegroom { get; set; }

    [Range(0.5, 5.0)]
    public decimal PriceMultiplier { get; set; } = 1.0m;
}
