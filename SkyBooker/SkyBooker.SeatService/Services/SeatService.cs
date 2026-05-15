using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkyBooker.SeatService.Common;
using SkyBooker.SeatService.DTOs;
using SkyBooker.SeatService.Entities;
using SkyBooker.SeatService.Repositories;

namespace SkyBooker.SeatService.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<SeatService> _logger;

    public SeatService(ISeatRepository repository, IMapper mapper, ILogger<SeatService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse<SeatDto>> GetSeatById(int seatId)
    {
        var seat = await _repository.FindBySeatId(seatId);
        if (seat == null)
        {
            return ApiResponse<SeatDto>.Failure("Seat not found", 404);
        }
        var seatDto = _mapper.Map<SeatDto>(seat);
        return ApiResponse<SeatDto>.Success(seatDto, "Seat retrieved successfully");
    }

    public async Task<ApiResponse<List<SeatDto>>> GetSeatsByFlightId(int flightId)
    {
        var seats = await _repository.FindByFlightId(flightId);
        var seatDtos = _mapper.Map<List<SeatDto>>(seats);
        return ApiResponse<List<SeatDto>>.Success(seatDtos, "Seats retrieved successfully");
    }

    public async Task<ApiResponse<List<SeatDto>>> GetSeatsByClass(int flightId, string seatClass)
    {
        var seats = await _repository.FindByFlightIdAndSeatClass(flightId, seatClass);
        var seatDtos = _mapper.Map<List<SeatDto>>(seats);
        return ApiResponse<List<SeatDto>>.Success(seatDtos, "Seats retrieved successfully");
    }

    public async Task<ApiResponse<List<SeatDto>>> GetAvailableSeats(int flightId)
    {
        var seats = await _repository.FindAvailableByFlightId(flightId);
        var seatDtos = _mapper.Map<List<SeatDto>>(seats);
        return ApiResponse<List<SeatDto>>.Success(seatDtos, "Available seats retrieved successfully");
    }

    public async Task<ApiResponse<List<SeatDto>>> GetAvailableByClass(int flightId, string seatClass)
    {
        var seats = await _repository.FindAvailableByClass(flightId, seatClass);
        var seatDtos = _mapper.Map<List<SeatDto>>(seats);
        return ApiResponse<List<SeatDto>>.Success(seatDtos, "Available seats retrieved successfully");
    }

    public async Task<ApiResponse<int>> CountAvailableByClass(int flightId, string seatClass)
    {
        var count = await _repository.CountAvailableByClass(flightId, seatClass);
        return ApiResponse<int>.Success(count, "Count retrieved successfully");
    }

    public async Task<ApiResponse<SeatDto>> AddSeat(CreateSeatDto createDto)
    {
        var existingSeat = await _repository.FindByFlightIdAndSeatNumber(createDto.FlightId, createDto.SeatNumber);
        if (existingSeat != null)
        {
            return ApiResponse<SeatDto>.Failure("Seat number already exists for this flight", 400);
        }

        var seat = _mapper.Map<Seat>(createDto);
        seat.Status = "AVAILABLE";
        seat.CreatedAt = DateTime.UtcNow;
        
        await _repository.AddSeat(seat);
        await _repository.SaveChangesAsync();

        var seatDto = _mapper.Map<SeatDto>(seat);
        return ApiResponse<SeatDto>.Success(seatDto, "Seat added successfully", 201);
    }

    public async Task<ApiResponse<SeatDto>> UpdateSeat(int seatId, UpdateSeatDto updateDto)
    {
        var seat = await _repository.FindBySeatId(seatId);
        if (seat == null)
        {
            return ApiResponse<SeatDto>.Failure("Seat not found", 404);
        }

        if (updateDto.Status != null)
        {
            seat.Status = updateDto.Status;
        }
        if (updateDto.PriceMultiplier.HasValue)
        {
            seat.PriceMultiplier = updateDto.PriceMultiplier.Value;
        }

        await _repository.UpdateSeat(seat);
        await _repository.SaveChangesAsync();

        var seatDto = _mapper.Map<SeatDto>(seat);
        return ApiResponse<SeatDto>.Success(seatDto, "Seat updated successfully");
    }

    public async Task<ApiResponse<bool>> HoldSeat(int seatId)
    {
        var seat = await _repository.FindBySeatId(seatId);
        if (seat == null)
        {
            return ApiResponse<bool>.Failure("Seat not found", 404);
        }

        if (!string.Equals(seat.Status, "AVAILABLE", StringComparison.OrdinalIgnoreCase))
        {
            return ApiResponse<bool>.Failure("Seat is not available", 400);
        }

        seat.Status = "HELD";
        seat.HeldSince = DateTime.UtcNow;
        
        await _repository.UpdateSeat(seat);
        await _repository.SaveChangesAsync();

        return ApiResponse<bool>.Success(true, "Seat held successfully");
    }

    public async Task<ApiResponse<bool>> ReleaseSeat(int seatId)
    {
        var seat = await _repository.FindBySeatId(seatId);
        if (seat == null)
        {
            return ApiResponse<bool>.Failure("Seat not found", 404);
        }

        if (seat.Status != "HELD")
        {
            return ApiResponse<bool>.Failure("Seat is not held", 400);
        }

        seat.Status = "AVAILABLE";
        seat.HeldSince = null;
        
        await _repository.UpdateSeat(seat);
        await _repository.SaveChangesAsync();

        return ApiResponse<bool>.Success(true, "Seat released successfully");
    }

    public async Task<ApiResponse<bool>> ConfirmSeat(int seatId)
    {
        var seat = await _repository.FindBySeatId(seatId);
        if (seat == null)
        {
            return ApiResponse<bool>.Failure("Seat not found", 404);
        }

        if (seat.Status != "HELD")
        {
            return ApiResponse<bool>.Failure("Seat is not held", 400);
        }

        seat.Status = "CONFIRMED";
        seat.HeldSince = null;
        seat.ConfirmedAt = DateTime.UtcNow;
        
        await _repository.UpdateSeat(seat);
        await _repository.SaveChangesAsync();

        return ApiResponse<bool>.Success(true, "Seat confirmed successfully");
    }

    public async Task<ApiResponse<bool>> DeleteSeat(int seatId)
    {
        var seat = await _repository.FindBySeatId(seatId);
        if (seat == null)
        {
            return ApiResponse<bool>.Failure("Seat not found", 404);
        }

        await _repository.DeleteSeat(seatId);
        await _repository.SaveChangesAsync();

        return ApiResponse<bool>.Success(true, "Seat deleted successfully");
    }

    public async Task<ApiResponse<bool>> DeleteSeatsForFlight(int flightId)
    {
        await _repository.DeleteSeatsForFlight(flightId);
        await _repository.SaveChangesAsync();

        return ApiResponse<bool>.Success(true, "Seats deleted successfully");
    }

    public async Task<ApiResponse<Dictionary<string, List<SeatDto>>>> GetSeatMap(int flightId)
    {
        var seats = await _repository.FindByFlightId(flightId);
        var seatDtos = _mapper.Map<List<SeatDto>>(seats);

        var seatMap = new Dictionary<string, List<SeatDto>>
        {
            { "Economy", seatDtos.Where(s => s.SeatClass == "Economy").ToList() },
            { "Business", seatDtos.Where(s => s.SeatClass == "Business").ToList() },
            { "First", seatDtos.Where(s => s.SeatClass == "First").ToList() }
        };

        return ApiResponse<Dictionary<string, List<SeatDto>>>.Success(seatMap, "Seat map retrieved successfully");
    }
}
