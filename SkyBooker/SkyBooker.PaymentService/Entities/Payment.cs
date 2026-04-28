using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyBooker.PaymentService.Entities;

[Table("Payments")]
public class Payment
{
    [Key]
    public string PaymentId { get; set; } = Guid.NewGuid().ToString();

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
    public string Status { get; set; } = "PENDING";

    [Required]
    [MaxLength(20)]
    public string PaymentMode { get; set; } = string.Empty;

    public string? TransactionId { get; set; }

    public string? GatewayResponse { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime? RefundedAt { get; set; }

    public decimal? RefundAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
