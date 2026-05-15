using SkyBooker.AirlineService.Entities;

namespace SkyBooker.AirlineService.Services;

public interface IAirlineService
{
    Task<Airline?> GetAirlineById(string airlineId);
    Task<Airline?> GetAirlineByIataCode(string iataCode);
    Task<List<Airline>> GetAllAirlines();
    Task<List<Airline>> GetActiveAirlines();
    Task<Airline> CreateAirline(Airline airline);
    Task<Airline> UpdateAirline(Airline airline);
    Task<bool> DeactivateAirline(string airlineId);
    
    // Airport methods
    Task<Airport?> GetAirportByIataCode(string iataCode);
    Task<List<Airport>> GetAirportsByCity(string city);
    Task<List<Airport>> GetAirportsByCountry(string country);
    Task<List<Airport>> SearchAirports(string searchTerm);
    Task<Airport> CreateAirport(Airport airport);
    Task<Airport> UpdateAirport(Airport airport);
}
