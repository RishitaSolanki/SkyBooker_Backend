using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyBooker.SeatService.Entities;

[Table("Seats")]
public class Seat
{
    [Key]
    public int SeatId { get; set; }

    [Required]
    public int FlightId { get; set; }

    [Required]
    [MaxLength(10)]
    public string SeatNumber { get; set; } = string.Empty; // e.g. 12A

    [Required]
    [MaxLength(20)]
    public string SeatClass { get; set; } = string.Empty; // Economy, Business, First

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

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "AVAILABLE"; // AVAILABLE, HELD, CONFIRMED, BLOCKED

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PriceMultiplier { get; set; } = 1.0m;

    public DateTime? HeldSince { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
