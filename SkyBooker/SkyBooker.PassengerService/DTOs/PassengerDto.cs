namespace SkyBooker.PassengerService.DTOs;

public class PassengerDto
{
    public string PassengerId { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string PassportNumber { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public DateTime PassportExpiry { get; set; }
    public string? SeatId { get; set; }
    public string? SeatNumber { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string PassengerType { get; set; } = "ADULT";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
