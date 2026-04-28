using Microsoft.EntityFrameworkCore;
using SkyBooker.BookingService.Data;
using SkyBooker.BookingService.Entities;

namespace SkyBooker.BookingService.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _context;

    public BookingRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> FindByUserId(string userId)
    {
        return await _context.Bookings.FirstOrDefaultAsync(b => b.UserId == userId);
    }

    public async Task<Booking?> FindByPnrCode(string pnrCode)
    {
        return await _context.Bookings.FirstOrDefaultAsync(b => b.PnrCode == pnrCode);
    }

    public async Task<Booking?> FindByFlightId(int flightId)
    {
        return await _context.Bookings.FirstOrDefaultAsync(b => b.FlightId == flightId);
    }

    public async Task<Booking?> FindByStatus(string status)
    {
        return await _context.Bookings.FirstOrDefaultAsync(b => b.Status == status);
    }

    public async Task<Booking?> FindByBookingId(string bookingId)
    {
        return await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    public async Task<int> CountByFlightIdAndStatus(int flightId, string status)
    {
        return await _context.Bookings.CountAsync(b => b.FlightId == flightId && b.Status == status);
    }

    public async Task<Booking?> FindByUserIdAndStatus(string userId, string status)
    {
        return await _context.Bookings.FirstOrDefaultAsync(b => b.UserId == userId && b.Status == status);
    }

    public async Task<List<Booking>> FindAllByUserId(string userId)
    {
        return await _context.Bookings.Where(b => b.UserId == userId).ToListAsync();
    }

    public async Task<List<Booking>> FindAllByFlightId(int flightId)
    {
        return await _context.Bookings.Where(b => b.FlightId == flightId).ToListAsync();
    }

    public async Task<List<Booking>> FindAllByStatus(string status)
    {
        return await _context.Bookings.Where(b => b.Status == status).ToListAsync();
    }

    public async Task Add(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
    }

    public async Task Update(Booking booking)
    {
        _context.Bookings.Update(booking);
        await Task.CompletedTask;
    }

    public async Task Delete(string bookingId)
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
