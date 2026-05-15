using Microsoft.EntityFrameworkCore;
using SkyBooker.SeatService.Data;
using SkyBooker.SeatService.Entities;

namespace SkyBooker.SeatService.Repositories;

public class SeatRepository : ISeatRepository
{
    private readonly SeatDbContext _context;

    public SeatRepository(SeatDbContext context)
    {
        _context = context;
    }

    public async Task<Seat?> FindBySeatId(int seatId)
    {
        return await _context.Seats.FirstOrDefaultAsync(s => s.SeatId == seatId);
    }

    public async Task<Seat?> FindByFlightIdAndSeatNumber(int flightId, string seatNumber)
    {
        return await _context.Seats.FirstOrDefaultAsync(s => s.FlightId == flightId && s.SeatNumber == seatNumber);
    }

    public async Task<List<Seat>> FindByFlightId(int flightId)
    {
        return await _context.Seats.Where(s => s.FlightId == flightId).ToListAsync();
    }

    public async Task<List<Seat>> FindByFlightIdAndSeatClass(int flightId, string seatClass)
    {
        return await _context.Seats
            .Where(s => s.FlightId == flightId && s.SeatClass == seatClass)
            .ToListAsync();
    }

    public async Task<List<Seat>> FindAvailableByFlightId(int flightId)
    {
        return await _context.Seats
            .Where(s => s.FlightId == flightId && s.Status == "AVAILABLE")
            .ToListAsync();
    }

    public async Task<List<Seat>> FindAvailableByClass(int flightId, string seatClass)
    {
        return await _context.Seats
            .Where(s => s.FlightId == flightId && s.SeatClass == seatClass && s.Status == "AVAILABLE")
            .ToListAsync();
    }

    public async Task<int> CountAvailableByClass(int flightId, string seatClass)
    {
        return await _context.Seats
            .CountAsync(s => s.FlightId == flightId && s.SeatClass == seatClass && s.Status == "AVAILABLE");
    }

    public async Task AddSeat(Seat seat)
    {
        await _context.Seats.AddAsync(seat);
    }

    public async Task UpdateSeat(Seat seat)
    {
        _context.Seats.Update(seat);
    }

    public async Task DeleteSeat(int seatId)
    {
        var seat = await FindBySeatId(seatId);
        if (seat != null)
        {
            _context.Seats.Remove(seat);
        }
    }

    public async Task DeleteSeatsForFlight(int flightId)
    {
        var seats = await FindByFlightId(flightId);
        _context.Seats.RemoveRange(seats);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
