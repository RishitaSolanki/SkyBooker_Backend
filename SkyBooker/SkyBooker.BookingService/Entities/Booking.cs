using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyBooker.BookingService.Entities;

[Table("Bookings")]
public class Booking
{
    [Key]
    public string BookingId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public int FlightId { get; set; }

    [Required]
    [MaxLength(6)]
    public string PnrCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string TripType { get; set; } = "ONE_WAY"; // ONE_WAY, ROUND_TRIP

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "PENDING"; // PENDING, CONFIRMED, CANCELLED, COMPLETED, NO_SHOW

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal BaseFare { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Taxes { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalFare { get; set; }

    [Required]
    [MaxLength(20)]
    public string MealPreference { get; set; } = "Veg"; // Veg, Non-Veg, Jain, Vegan

    [Required]
    public int LuggageKg { get; set; } = 15;

    [Required]
    [MaxLength(100)]
    public string ContactEmail { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string ContactPhone { get; set; } = string.Empty;

    [Required]
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;

    public string? PaymentId { get; set; }
}
