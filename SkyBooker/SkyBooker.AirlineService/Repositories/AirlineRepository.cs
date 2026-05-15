using Microsoft.EntityFrameworkCore;
using SkyBooker.AirlineService.Data;
using SkyBooker.AirlineService.Entities;

namespace SkyBooker.AirlineService.Repositories;

public class AirlineRepository : IAirlineRepository
{
    private readonly AirlineDbContext _context;

    public AirlineRepository(AirlineDbContext context)
    {
        _context = context;
    }

    public async Task<Airline?> FindByAirlineId(string airlineId)
    {
        return await _context.Airlines.FirstOrDefaultAsync(a => a.AirlineId == airlineId);
    }

    public async Task<Airline?> FindByIataCode(string iataCode)
    {
        return await _context.Airlines.FirstOrDefaultAsync(a => a.IataCode == iataCode);
    }

    public async Task<List<Airline>> FindByIsActive(bool isActive)
    {
        return await _context.Airlines.Where(a => a.IsActive == isActive).ToListAsync();
    }

    public async Task<List<Airline>> GetAllAirlines()
    {
        return await _context.Airlines.ToListAsync();
    }

    public async Task<Airline> AddAirline(Airline airline)
    {
        _context.Airlines.Add(airline);
        await _context.SaveChangesAsync();
        return airline;
    }

    public async Task<Airline> UpdateAirline(Airline airline)
    {
        airline.UpdatedAt = DateTime.UtcNow;
        _context.Airlines.Update(airline);
        await _context.SaveChangesAsync();
        return airline;
    }

    public async Task<bool> DeactivateAirline(string airlineId)
    {
        var airline = await FindByAirlineId(airlineId);
        if (airline == null)
            return false;

        airline.IsActive = false;
        airline.UpdatedAt = DateTime.UtcNow;
        _context.Airlines.Update(airline);
        await _context.SaveChangesAsync();
        return true;
    }

    // Airport methods
    public async Task<Airport?> FindAirportByIataCode(string iataCode)
    {
        return await _context.Airports.FirstOrDefaultAsync(a => a.IataCode == iataCode);
    }

    public async Task<List<Airport>> FindAirportsByCity(string city)
    {
        return await _context.Airports.Where(a => EF.Functions.Like(a.City, $"%{city}%")).ToListAsync();
    }

    public async Task<List<Airport>> FindAirportsByCountry(string country)
    {
        return await _context.Airports.Where(a => EF.Functions.Like(a.Country, $"%{country}%")).ToListAsync();
    }

    public async Task<List<Airport>> SearchAirports(string searchTerm)
    {
        return await _context.Airports
            .Where(a => EF.Functions.Like(a.Name, $"%{searchTerm}%") ||
                        EF.Functions.Like(a.City, $"%{searchTerm}%") ||
                        EF.Functions.Like(a.IataCode, $"%{searchTerm}%"))
            .ToListAsync();
    }

    public async Task<Airport> AddAirport(Airport airport)
    {
        _context.Airports.Add(airport);
        await _context.SaveChangesAsync();
        return airport;
    }

    public async Task<Airport> UpdateAirport(Airport airport)
    {
        airport.UpdatedAt = DateTime.UtcNow;
        _context.Airports.Update(airport);
        await _context.SaveChangesAsync();
        return airport;
    }
}
