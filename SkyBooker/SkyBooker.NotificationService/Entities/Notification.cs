using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyBooker.NotificationService.Entities;

public class Notification
{
    [Key]
    public string NotificationId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [StringLength(50)]
    public string RecipientId { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Type { get; set; } = string.Empty; // BOOKING_CONFIRMED/FLIGHT_DELAY/GATE_CHANGE/CHECKIN_REMINDER/BOARDING/CANCELLATION

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Channel { get; set; } = string.Empty; // APP/EMAIL/SMS

    [StringLength(50)]
    public string? RelatedBookingId { get; set; }

    [Required]
    public bool IsRead { get; set; } = false;

    [Required]
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "datetime")]
    public DateTime? ReadAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }
}
