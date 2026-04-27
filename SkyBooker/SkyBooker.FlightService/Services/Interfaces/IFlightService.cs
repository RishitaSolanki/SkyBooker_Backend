using SkyBooker.FlightService.Common;
using SkyBooker.FlightService.DTOs;

namespace SkyBooker.FlightService.Services.Interfaces;

public interface IFlightService
{
    Task<ApiResponse<FlightDto>> GetFlightByIdAsync(int flightId);
    Task<ApiResponse<FlightDto>> GetFlightByNumberAsync(string flightNumber);
    Task<ApiResponse<IEnumerable<FlightDto>>> GetAllFlightsAsync();
    Task<ApiResponse<IEnumerable<FlightDto>>> SearchFlightsAsync(SearchFlightDto searchDto);
    Task<ApiResponse<Dictionary<string, IEnumerable<FlightDto>>>> SearchRoundTripAsync(RoundTripSearchDto searchDto);
    Task<ApiResponse<IEnumerable<FlightDto>>> GetFlightsByAirlineAsync(int airlineId);
    Task<ApiResponse<FlightDto>> CreateFlightAsync(CreateFlightDto createDto);
    Task<ApiResponse<FlightDto>> UpdateFlightAsync(int flightId, UpdateFlightDto updateDto);
    Task<ApiResponse<bool>> UpdateFlightStatusAsync(int flightId, string status);
    Task<ApiResponse<bool>> DeleteFlightAsync(int flightId);
    Task<ApiResponse<bool>> DecrementSeatsAsync(int flightId, int count);
    Task<ApiResponse<bool>> IncrementSeatsAsync(int flightId, int count);
}