using SkyBooker.FlightService.Models;

namespace SkyBooker.FlightService.Repositories.Interfaces;

public interface IFlightRepository
{
    Task<Flight?> GetByIdAsync(int flightId);
    Task<Flight?> GetByFlightNumberAsync(string flightNumber);
    Task<IEnumerable<Flight>> GetAllAsync();
    Task<IEnumerable<Flight>> SearchFlightsAsync(
        string origin, 
        string destination, 
        DateTime date,
        int passengers);
    Task<IEnumerable<Flight>> GetFlightsByAirlineIdAsync(int airlineId);
    Task<IEnumerable<Flight>> GetFlightsByStatusAsync(string status);
    Task<int> GetCountByAirlineIdAsync(int airlineId);
    Task<Flight> AddAsync(Flight flight);
    Task UpdateAsync(Flight flight);
    Task DeleteAsync(int flightId);
    Task<bool> ExistsByFlightNumberAsync(string flightNumber);
    Task<bool> DecrementAvailableSeatsAsync(int flightId, int count);
    Task<bool> IncrementAvailableSeatsAsync(int flightId, int count);
}