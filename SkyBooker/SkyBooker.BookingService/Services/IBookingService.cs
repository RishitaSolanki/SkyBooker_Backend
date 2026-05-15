using SkyBooker.BookingService.DTOs;

namespace SkyBooker.BookingService.Services;

public interface IBookingService
{
    Task<BookingDto?> CreateBooking(CreateBookingDto request);
    Task<BookingDto?> GetBookingById(string bookingId);
    Task<BookingDto?> GetBookingByPnr(string pnrCode);
    Task<List<BookingDto>> GetBookingsByUser(string userId);
    Task<List<BookingDto>> GetBookingsByFlight(int flightId);
    Task<bool> CancelBooking(string bookingId);
    Task<bool> UpdateStatus(string bookingId, string status);
    Task<FareSummary> CalculateFare(CreateBookingDto request);
    Task<string> GeneratePnr();
    Task<List<BookingDto>> GetUpcomingBookings(string userId);
    Task<int> CancelBookingsByFlight(int flightId);
}
