using SkyBooker.AirlineService.Entities;
using SkyBooker.AirlineService.Repositories;

namespace SkyBooker.AirlineService.Services;

public class AirlineService : IAirlineService
{
    private readonly IAirlineRepository _repository;

    public AirlineService(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task<Airline?> GetAirlineById(string airlineId)
    {
        return await _repository.FindByAirlineId(airlineId);
    }

    public async Task<Airline?> GetAirlineByIataCode(string iataCode)
    {
        return await _repository.FindByIataCode(iataCode);
    }

    public async Task<List<Airline>> GetAllAirlines()
    {
        return await _repository.GetAllAirlines();
    }

    public async Task<List<Airline>> GetActiveAirlines()
    {
        return await _repository.FindByIsActive(true);
    }

    public async Task<Airline> CreateAirline(Airline airline)
    {
        return await _repository.AddAirline(airline);
    }

    public async Task<Airline> UpdateAirline(Airline airline)
    {
        return await _repository.UpdateAirline(airline);
    }

    public async Task<bool> DeactivateAirline(string airlineId)
    {
        return await _repository.DeactivateAirline(airlineId);
    }
    
    // Airport methods
    public async Task<Airport?> GetAirportByIataCode(string iataCode)
    {
        return await _repository.FindAirportByIataCode(iataCode);
    }

    public async Task<List<Airport>> GetAirportsByCity(string city)
    {
        return await _repository.FindAirportsByCity(city);
    }

    public async Task<List<Airport>> GetAirportsByCountry(string country)
    {
        return await _repository.FindAirportsByCountry(country);
    }

    public async Task<List<Airport>> SearchAirports(string searchTerm)
    {
        return await _repository.SearchAirports(searchTerm);
    }

    public async Task<Airport> CreateAirport(Airport airport)
    {
        return await _repository.AddAirport(airport);
    }

    public async Task<Airport> UpdateAirport(Airport airport)
    {
        return await _repository.UpdateAirport(airport);
    }
}
