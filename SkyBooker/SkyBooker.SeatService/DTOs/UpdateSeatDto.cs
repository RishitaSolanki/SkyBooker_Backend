using System.ComponentModel.DataAnnotations;

namespace SkyBooker.SeatService.DTOs;

public class UpdateSeatDto
{
    [MaxLength(20)]
    public string? Status { get; set; }

    [Range(0.5, 5.0)]
    public decimal? PriceMultiplier { get; set; }
}
