using Microsoft.EntityFrameworkCore;
using SkyBooker.FlightService.Data;
using SkyBooker.FlightService.Models;
using SkyBooker.FlightService.Repositories.Interfaces;

namespace SkyBooker.FlightService.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly FlightDbContext _context;

    public FlightRepository(FlightDbContext context)
    {
        _context = context;
    }

    public async Task<Flight?> GetByIdAsync(int flightId)
    {
        return await _context.Flights
            .Include(f => f.Airline)
            .FirstOrDefaultAsync(f => f.FlightId == flightId);
    }

    public async Task<Flight?> GetByFlightNumberAsync(string flightNumber)
    {
        return await _context.Flights
            .Include(f => f.Airline)
            .FirstOrDefaultAsync(f => f.FlightNumber == flightNumber);
    }

    public async Task<IEnumerable<Flight>> GetAllAsync()
    {
        return await _context.Flights
            .Include(f => f.Airline)
            .OrderBy(f => f.DepartureTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Flight>> SearchFlightsAsync(
        string origin, 
        string destination, 
        DateTime date, 
        int passengers)
    {
        return await _context.Flights
            .Include(f => f.Airline)
            .Where(f => f.OriginAirportCode == origin 
                     && f.DestinationAirportCode == destination
                     && f.DepartureTime.Date == date.Date
                     && f.AvailableSeats >= passengers
                     && f.Status != Common.Enums.FlightStatus.Cancelled)
            .OrderBy(f => f.DepartureTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Flight>> GetFlightsByAirlineIdAsync(int airlineId)
    {
        return await _context.Flights
            .Include(f => f.Airline)
            .Where(f => f.AirlineId == airlineId)
            .OrderBy(f => f.DepartureTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Flight>> GetFlightsByStatusAsync(string status)
    {
        if (Enum.TryParse<Common.Enums.FlightStatus>(status, true, out var flightStatus))
        {
            return await _context.Flights
                .Include(f => f.Airline)
                .Where(f => f.Status == flightStatus)
                .OrderBy(f => f.DepartureTime)
                .ToListAsync();
        }

        return Enumerable.Empty<Flight>();
    }

    public async Task<int> GetCountByAirlineIdAsync(int airlineId)
    {
        return await _context.Flights
            .CountAsync(f => f.AirlineId == airlineId);
    }

    public async Task<Flight> AddAsync(Flight flight)
    {
        flight.CreatedAt = DateTime.UtcNow;
        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();
        return flight;
    }

    public async Task UpdateAsync(Flight flight)
    {
        flight.UpdatedAt = DateTime.UtcNow;
        _context.Flights.Update(flight);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int flightId)
    {
        var flight = await _context.Flights.FindAsync(flightId);
        if (flight != null)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByFlightNumberAsync(string flightNumber)
    {
        return await _context.Flights
            .AnyAsync(f => f.FlightNumber == flightNumber);
    }

    public async Task<bool> DecrementAvailableSeatsAsync(int flightId, int count)
    {
        var flight = await _context.Flights.FindAsync(flightId);
        if (flight == null || flight.AvailableSeats < count)
            return false;

        flight.AvailableSeats -= count;
        flight.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IncrementAvailableSeatsAsync(int flightId, int count)
    {
        var flight = await _context.Flights.FindAsync(flightId);
        if (flight == null || flight.AvailableSeats + count > flight.TotalSeats)
            return false;

        flight.AvailableSeats += count;
        flight.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}