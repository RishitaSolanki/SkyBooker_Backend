using System.ComponentModel.DataAnnotations;

namespace SkyBooker.PaymentService.DTOs;

public class CreatePaymentDto
{
    [Required]
    public string BookingId { get; set; } = string.Empty;

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }

    [Required]
    [MaxLength(10)]
    public string Currency { get; set; } = "INR";

    [Required]
    [MaxLength(20)]
    public string PaymentMode { get; set; } = string.Empty;

    public string? TransactionId { get; set; }
}
