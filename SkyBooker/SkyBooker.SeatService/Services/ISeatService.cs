using SkyBooker.SeatService.Common;
using SkyBooker.SeatService.DTOs;

namespace SkyBooker.SeatService.Services;

public interface ISeatService
{
    Task<ApiResponse<SeatDto>> GetSeatById(int seatId);
    Task<ApiResponse<List<SeatDto>>> GetSeatsByFlightId(int flightId);
    Task<ApiResponse<List<SeatDto>>> GetSeatsByClass(int flightId, string seatClass);
    Task<ApiResponse<List<SeatDto>>> GetAvailableSeats(int flightId);
    Task<ApiResponse<List<SeatDto>>> GetAvailableByClass(int flightId, string seatClass);
    Task<ApiResponse<int>> CountAvailableByClass(int flightId, string seatClass);
    Task<ApiResponse<SeatDto>> AddSeat(CreateSeatDto createDto);
    Task<ApiResponse<SeatDto>> UpdateSeat(int seatId, UpdateSeatDto updateDto);
    Task<ApiResponse<bool>> HoldSeat(int seatId);
    Task<ApiResponse<bool>> ReleaseSeat(int seatId);
    Task<ApiResponse<bool>> ConfirmSeat(int seatId);
    Task<ApiResponse<bool>> DeleteSeat(int seatId);
    Task<ApiResponse<bool>> DeleteSeatsForFlight(int flightId);
    Task<ApiResponse<Dictionary<string, List<SeatDto>>>> GetSeatMap(int flightId);
}
