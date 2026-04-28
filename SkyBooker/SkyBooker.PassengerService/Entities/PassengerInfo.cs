using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyBooker.PassengerService.Entities;

[Table("PassengerInfos")]
public class PassengerInfo
{
    [Key]
    public string PassengerId { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string BookingId { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [MaxLength(10)]
    public string Gender { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string PassportNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Nationality { get; set; } = string.Empty;

    public DateTime PassportExpiry { get; set; }

    public string? SeatId { get; set; }

    public string? SeatNumber { get; set; }

    [Required]
    [MaxLength(50)]
    public string TicketNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string PassengerType { get; set; } = "ADULT";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
