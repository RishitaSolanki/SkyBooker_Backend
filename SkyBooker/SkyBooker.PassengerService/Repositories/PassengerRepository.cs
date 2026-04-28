using Microsoft.EntityFrameworkCore;
using SkyBooker.PassengerService.Data;
using SkyBooker.PassengerService.Entities;

namespace SkyBooker.PassengerService.Repositories;

public class PassengerRepository : IPassengerRepository
{
    private readonly PassengerDbContext _context;

    public PassengerRepository(PassengerDbContext context)
    {
        _context = context;
    }

    public async Task<PassengerInfo?> FindByPassengerId(string passengerId)
    {
        return await _context.PassengerInfos.FirstOrDefaultAsync(p => p.PassengerId == passengerId);
    }

    public async Task<PassengerInfo?> FindByBookingId(string bookingId)
    {
        return await _context.PassengerInfos.FirstOrDefaultAsync(p => p.BookingId == bookingId);
    }

    public async Task<PassengerInfo?> FindByPassportNumber(string passportNumber)
    {
        return await _context.PassengerInfos.FirstOrDefaultAsync(p => p.PassportNumber == passportNumber);
    }

    public async Task<PassengerInfo?> FindByTicketNumber(string ticketNumber)
    {
        return await _context.PassengerInfos.FirstOrDefaultAsync(p => p.TicketNumber == ticketNumber);
    }

    public async Task<PassengerInfo?> FindBySeatId(string seatId)
    {
        return await _context.PassengerInfos.FirstOrDefaultAsync(p => p.SeatId == seatId);
    }

    public async Task<List<PassengerInfo>> FindByBookingIdList(string bookingId)
    {
        return await _context.PassengerInfos.Where(p => p.BookingId == bookingId).ToListAsync();
    }

    public async Task<int> CountByBookingId(string bookingId)
    {
        return await _context.PassengerInfos.CountAsync(p => p.BookingId == bookingId);
    }

    public async Task<bool> DeleteByBookingId(string bookingId)
    {
        var passengers = await _context.PassengerInfos.Where(p => p.BookingId == bookingId).ToListAsync();
        if (passengers.Any())
        {
            _context.PassengerInfos.RemoveRange(passengers);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<PassengerInfo> Add(PassengerInfo passenger)
    {
        _context.PassengerInfos.Add(passenger);
        await _context.SaveChangesAsync();
        return passenger;
    }

    public async Task<PassengerInfo> Update(PassengerInfo passenger)
    {
        passenger.UpdatedAt = DateTime.UtcNow;
        _context.PassengerInfos.Update(passenger);
        await _context.SaveChangesAsync();
        return passenger;
    }

    public async Task<bool> Delete(string passengerId)
    {
        var passenger = await _context.PassengerInfos.FirstOrDefaultAsync(p => p.PassengerId == passengerId);
        if (passenger != null)
        {
            _context.PassengerInfos.Remove(passenger);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
