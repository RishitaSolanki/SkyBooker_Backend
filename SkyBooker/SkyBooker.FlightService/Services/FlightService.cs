using AutoMapper;
using SkyBooker.FlightService.Common;
using SkyBooker.FlightService.Common.Enums;
using SkyBooker.FlightService.DTOs;
using SkyBooker.FlightService.Models;
using SkyBooker.FlightService.Repositories.Interfaces;
using SkyBooker.FlightService.Services.Interfaces;

namespace SkyBooker.FlightService.Services;

public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<FlightService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public FlightService(
        IFlightRepository flightRepository,
        IMapper mapper,
        ILogger<FlightService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _flightRepository = flightRepository;
        _mapper = mapper;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ApiResponse<FlightDto>> GetFlightByIdAsync(int flightId)
    {
        try
        {
            var flight = await _flightRepository.GetByIdAsync(flightId);
            if (flight == null)
            {
                _logger.LogWarning("Flight with ID {FlightId} not found", flightId);
                return ApiResponse<FlightDto>.Failure("Flight not found", 404);
            }

            var flightDto = _mapper.Map<FlightDto>(flight);
            return ApiResponse<FlightDto>.Success(flightDto, "Flight retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving flight with ID {FlightId}", flightId);
            return ApiResponse<FlightDto>.Failure("An error occurred while retrieving the flight", 500);
        }
    }

    public async Task<ApiResponse<FlightDto>> GetFlightByNumberAsync(string flightNumber)
    {
        try
        {
            var flight = await _flightRepository.GetByFlightNumberAsync(flightNumber);
            if (flight == null)
            {
                _logger.LogWarning("Flight with number {FlightNumber} not found", flightNumber);
                return ApiResponse<FlightDto>.Failure("Flight not found", 404);
            }

            var flightDto = _mapper.Map<FlightDto>(flight);
            return ApiResponse<FlightDto>.Success(flightDto, "Flight retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving flight with number {FlightNumber}", flightNumber);
            return ApiResponse<FlightDto>.Failure("An error occurred while retrieving the flight", 500);
        }
    }

    public async Task<ApiResponse<IEnumerable<FlightDto>>> GetAllFlightsAsync()
    {
        try
        {
            var flights = await _flightRepository.GetAllAsync();
            var flightDtos = _mapper.Map<IEnumerable<FlightDto>>(flights);
            return ApiResponse<IEnumerable<FlightDto>>.Success(flightDtos, "Flights retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all flights");
            return ApiResponse<IEnumerable<FlightDto>>.Failure("An error occurred while retrieving flights", 500);
        }
    }

    public async Task<ApiResponse<IEnumerable<FlightDto>>> SearchFlightsAsync(SearchFlightDto searchDto)
    {
        try
        {
            var flights = await _flightRepository.SearchFlightsAsync(
                searchDto.OriginAirportCode,
                searchDto.DestinationAirportCode,
                searchDto.DepartureDate,
                searchDto.Passengers);

            // Apply filters if provided
            if (searchDto.Filters != null)
            {
                flights = ApplyFilters(flights, searchDto.Filters);
            }

            var flightDtos = _mapper.Map<IEnumerable<FlightDto>>(flights);
            return ApiResponse<IEnumerable<FlightDto>>.Success(
                flightDtos, 
                $"Found {flightDtos.Count()} flights");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching flights");
            return ApiResponse<IEnumerable<FlightDto>>.Failure("An error occurred while searching flights", 500);
        }
    }

    public async Task<ApiResponse<Dictionary<string, IEnumerable<FlightDto>>>> SearchRoundTripAsync(RoundTripSearchDto searchDto)
    {
        try
        {
            // Search outbound flights
            var outboundFlights = await _flightRepository.SearchFlightsAsync(
                searchDto.OriginAirportCode,
                searchDto.DestinationAirportCode,
                searchDto.DepartureDate,
                searchDto.Passengers);

            // Search return flights
            var returnFlights = await _flightRepository.SearchFlightsAsync(
                searchDto.DestinationAirportCode,
                searchDto.OriginAirportCode,
                searchDto.ReturnDate,
                searchDto.Passengers);

            // Apply filters if provided
            if (searchDto.Filters != null)
            {
                outboundFlights = ApplyFilters(outboundFlights, searchDto.Filters);
                returnFlights = ApplyFilters(returnFlights, searchDto.Filters);
            }

            var result = new Dictionary<string, IEnumerable<FlightDto>>
            {
                ["outbound"] = _mapper.Map<IEnumerable<FlightDto>>(outboundFlights),
                ["return"] = _mapper.Map<IEnumerable<FlightDto>>(returnFlights)
            };

            return ApiResponse<Dictionary<string, IEnumerable<FlightDto>>>.Success(
                result, 
                $"Found {result["outbound"].Count()} outbound and {result["return"].Count()} return flights");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching round-trip flights");
            return ApiResponse<Dictionary<string, IEnumerable<FlightDto>>>.Failure(
                "An error occurred while searching round-trip flights", 500);
        }
    }

    public async Task<ApiResponse<IEnumerable<FlightDto>>> GetFlightsByAirlineAsync(int airlineId)
    {
        try
        {
            var flights = await _flightRepository.GetFlightsByAirlineIdAsync(airlineId);
            var flightDtos = _mapper.Map<IEnumerable<FlightDto>>(flights);
            return ApiResponse<IEnumerable<FlightDto>>.Success(flightDtos, "Flights retrieved successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving flights for airline {AirlineId}", airlineId);
            return ApiResponse<IEnumerable<FlightDto>>.Failure("An error occurred while retrieving flights", 500);
        }
    }

    public async Task<ApiResponse<FlightDto>> CreateFlightAsync(CreateFlightDto createDto)
    {
        try
        {
            // Validate flight number uniqueness
            if (await _flightRepository.ExistsByFlightNumberAsync(createDto.FlightNumber))
            {
                return ApiResponse<FlightDto>.Failure("Flight number already exists", 409);
            }

            // Validate departure time is in the future
            if (createDto.DepartureTime <= DateTime.UtcNow)
            {
                return ApiResponse<FlightDto>.Failure("Departure time must be in the future", 400);
            }

            // Validate arrival time is after departure time
            if (createDto.ArrivalTime <= createDto.DepartureTime)
            {
                return ApiResponse<FlightDto>.Failure("Arrival time must be after departure time", 400);
            }

            var flight = _mapper.Map<Flight>(createDto);
            var createdFlight = await _flightRepository.AddAsync(flight);
            var flightDto = _mapper.Map<FlightDto>(createdFlight);

            _logger.LogInformation("Flight {FlightNumber} created successfully", createdFlight.FlightNumber);
            return ApiResponse<FlightDto>.Success(flightDto, "Flight created successfully", 201);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating flight");
            return ApiResponse<FlightDto>.Failure("An error occurred while creating the flight", 500);
        }
    }

    public async Task<ApiResponse<FlightDto>> UpdateFlightAsync(int flightId, UpdateFlightDto updateDto)
    {
        try
        {
            var flight = await _flightRepository.GetByIdAsync(flightId);
            if (flight == null)
            {
                return ApiResponse<FlightDto>.Failure("Flight not found", 404);
            }

            // Apply updates
            _mapper.Map(updateDto, flight);
            
            // Recalculate duration if times changed
            if (updateDto.DepartureTime.HasValue || updateDto.ArrivalTime.HasValue)
            {
                flight.DurationMinutes = (int)(flight.ArrivalTime - flight.DepartureTime).TotalMinutes;
            }

            await _flightRepository.UpdateAsync(flight);
            var flightDto = _mapper.Map<FlightDto>(flight);

            _logger.LogInformation("Flight {FlightNumber} updated successfully", flight.FlightNumber);
            return ApiResponse<FlightDto>.Success(flightDto, "Flight updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating flight {FlightId}", flightId);
            return ApiResponse<FlightDto>.Failure("An error occurred while updating the flight", 500);
        }
    }

    public async Task<ApiResponse<bool>> UpdateFlightStatusAsync(int flightId, string status)
    {
        try
        {
            var flight = await _flightRepository.GetByIdAsync(flightId);
            if (flight == null)
            {
                return ApiResponse<bool>.Failure("Flight not found", 404);
            }

            if (!Enum.TryParse<FlightStatus>(status, true, out var flightStatus))
            {
                return ApiResponse<bool>.Failure("Invalid flight status", 400);
            }

            flight.Status = flightStatus;
            await _flightRepository.UpdateAsync(flight);

            _logger.LogInformation("Flight {FlightNumber} status updated to {Status}", flight.FlightNumber, status);
            return ApiResponse<bool>.Success(true, "Flight status updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating flight status for flight {FlightId}", flightId);
            return ApiResponse<bool>.Failure("An error occurred while updating flight status", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteFlightAsync(int flightId)
    {
        try
        {
            var flight = await _flightRepository.GetByIdAsync(flightId);
            if (flight == null)
            {
                return ApiResponse<bool>.Failure("Flight not found", 404);
            }

            await _flightRepository.DeleteAsync(flightId);
            
            // Notify Booking Service to cancel all bookings for this flight
            try 
            {
                var client = _httpClientFactory.CreateClient();
                await client.PutAsync($"http://localhost:5214/api/booking/flight/{flightId}/cancel", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to notify BookingService about deleted flight {FlightId}", flightId);
            }

            _logger.LogInformation("Flight {FlightNumber} deleted successfully", flight.FlightNumber);
            return ApiResponse<bool>.Success(true, "Flight deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting flight {FlightId}", flightId);
            return ApiResponse<bool>.Failure("An error occurred while deleting the flight", 500);
        }
    }

    public async Task<ApiResponse<bool>> DecrementSeatsAsync(int flightId, int count)
    {
        try
        {
            var success = await _flightRepository.DecrementAvailableSeatsAsync(flightId, count);
            if (!success)
            {
                return ApiResponse<bool>.Failure("Unable to decrement seats. Flight not found or insufficient seats available", 400);
            }

            return ApiResponse<bool>.Success(true, "Seats decremented successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error decrementing seats for flight {FlightId}", flightId);
            return ApiResponse<bool>.Failure("An error occurred while decrementing seats", 500);
        }
    }

    public async Task<ApiResponse<bool>> IncrementSeatsAsync(int flightId, int count)
    {
        try
        {
            var success = await _flightRepository.IncrementAvailableSeatsAsync(flightId, count);
            if (!success)
            {
                return ApiResponse<bool>.Failure("Unable to increment seats. Flight not found or exceeds total seats", 400);
            }

            return ApiResponse<bool>.Success(true, "Seats incremented successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error incrementing seats for flight {FlightId}", flightId);
            return ApiResponse<bool>.Failure("An error occurred while incrementing seats", 500);
        }
    }

    // Helper method to apply filters
    private IEnumerable<Flight> ApplyFilters(IEnumerable<Flight> flights, FlightFilterDto filters)
    {
        if (filters.MinPrice.HasValue)
            flights = flights.Where(f => f.EconomyPrice >= filters.MinPrice.Value);

        if (filters.MaxPrice.HasValue)
            flights = flights.Where(f => f.EconomyPrice <= filters.MaxPrice.Value);

        if (filters.AirlineId.HasValue)
            flights = flights.Where(f => f.AirlineId == filters.AirlineId.Value);

        if (filters.EarliestDepartureTime.HasValue)
            flights = flights.Where(f => f.DepartureTime.TimeOfDay >= filters.EarliestDepartureTime.Value);

        if (filters.LatestDepartureTime.HasValue)
            flights = flights.Where(f => f.DepartureTime.TimeOfDay <= filters.LatestDepartureTime.Value);

        if (filters.EarliestArrivalTime.HasValue)
            flights = flights.Where(f => f.ArrivalTime.TimeOfDay >= filters.EarliestArrivalTime.Value);

        if (filters.LatestArrivalTime.HasValue)
            flights = flights.Where(f => f.ArrivalTime.TimeOfDay <= filters.LatestArrivalTime.Value);

        return flights;
    }
}