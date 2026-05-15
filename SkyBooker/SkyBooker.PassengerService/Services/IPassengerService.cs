using SkyBooker.PassengerService.DTOs;
using SkyBooker.PassengerService.Entities;

namespace SkyBooker.PassengerService.Services;

public interface IPassengerService
{
    Task<PassengerDto?> GetPassengerById(string passengerId);
    Task<List<PassengerDto>> GetPassengersByBooking(string bookingId);
    Task<PassengerDto?> GetByPassportNumber(string passportNumber);
    Task<PassengerDto?> GetByTicketNumber(string ticketNumber);
    Task<PassengerDto?> GetBySeatId(string seatId);
    Task<int> GetPassengerCount(string bookingId);
    Task<PassengerDto?> AddPassenger(CreatePassengerDto request);
    Task<PassengerDto?> UpdatePassenger(string passengerId, CreatePassengerDto request);
    Task<PassengerDto?> AssignSeat(string passengerId, string seatId, string seatNumber);
    Task<bool> DeletePassenger(string passengerId);
    Task<bool> DeletePassengersByBooking(string bookingId);
    string GenerateTicketNumber(string airlineCode, string flightNumber);
    bool ValidatePassengerData(CreatePassengerDto request);
}
