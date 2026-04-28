using System.ComponentModel.DataAnnotations;

namespace SkyBooker.NotificationService.DTOs;

public class CreateNotificationDto
{
    [Required]
    [StringLength(50)]
    public string RecipientId { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Type { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Channel { get; set; } = string.Empty;

    [StringLength(50)]
    public string? RelatedBookingId { get; set; }
}
