using SkyBooker.AirlineService.Entities;

namespace SkyBooker.AirlineService.Repositories;

public interface IAirlineRepository
{
    Task<Airline?> FindByAirlineId(string airlineId);
    Task<Airline?> FindByIataCode(string iataCode);
    Task<List<Airline>> FindByIsActive(bool isActive);
    Task<List<Airline>> GetAllAirlines();
    Task<Airline> AddAirline(Airline airline);
    Task<Airline> UpdateAirline(Airline airline);
    Task<bool> DeactivateAirline(string airlineId);
    
    // Airport methods
    Task<Airport?> FindAirportByIataCode(string iataCode);
    Task<List<Airport>> FindAirportsByCity(string city);
    Task<List<Airport>> FindAirportsByCountry(string country);
    Task<List<Airport>> SearchAirports(string searchTerm);
    Task<Airport> AddAirport(Airport airport);
    Task<Airport> UpdateAirport(Airport airport);
}
