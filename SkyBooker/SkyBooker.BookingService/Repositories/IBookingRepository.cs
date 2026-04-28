using SkyBooker.BookingService.Entities;

namespace SkyBooker.BookingService.Repositories;

public interface IBookingRepository
{
    Task<Booking?> FindByUserId(string userId);
    Task<Booking?> FindByPnrCode(string pnrCode);
    Task<Booking?> FindByFlightId(int flightId);
    Task<Booking?> FindByStatus(string status);
    Task<Booking?> FindByBookingId(string bookingId);
    Task<int> CountByFlightIdAndStatus(int flightId, string status);
    Task<Booking?> FindByUserIdAndStatus(string userId, string status);
    Task<List<Booking>> FindAllByUserId(string userId);
    Task<List<Booking>> FindAllByFlightId(int flightId);
    Task<List<Booking>> FindAllByStatus(string status);
    Task Add(Booking booking);
    Task Update(Booking booking);
    Task Delete(string bookingId);
    Task SaveChangesAsync();
}
