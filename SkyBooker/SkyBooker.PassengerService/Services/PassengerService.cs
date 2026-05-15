using SkyBooker.PassengerService.DTOs;
using SkyBooker.PassengerService.Entities;
using SkyBooker.PassengerService.Repositories;

namespace SkyBooker.PassengerService.Services;

public class PassengerService : IPassengerService
{
    private readonly IPassengerRepository _repository;

    public PassengerService(IPassengerRepository repository)
    {
        _repository = repository;
    }

    public async Task<PassengerDto?> GetPassengerById(string passengerId)
    {
        var passenger = await _repository.FindByPassengerId(passengerId);
        return passenger != null ? MapToDto(passenger) : null;
    }

    public async Task<List<PassengerDto>> GetPassengersByBooking(string bookingId)
    {
        var passengers = await _repository.FindByBookingIdList(bookingId);
        return passengers.Select(MapToDto).ToList();
    }

    public async Task<PassengerDto?> GetByPassportNumber(string passportNumber)
    {
        var passenger = await _repository.FindByPassportNumber(passportNumber);
        return passenger != null ? MapToDto(passenger) : null;
    }

    public async Task<PassengerDto?> GetByTicketNumber(string ticketNumber)
    {
        var passenger = await _repository.FindByTicketNumber(ticketNumber);
        return passenger != null ? MapToDto(passenger) : null;
    }

    public async Task<PassengerDto?> GetBySeatId(string seatId)
    {
        var passenger = await _repository.FindBySeatId(seatId);
        return passenger != null ? MapToDto(passenger) : null;
    }

    public async Task<int> GetPassengerCount(string bookingId)
    {
        return await _repository.CountByBookingId(bookingId);
    }

    public async Task<PassengerDto?> AddPassenger(CreatePassengerDto request)
    {
        if (!ValidatePassengerData(request))
            return null;

        var passenger = new PassengerInfo
        {
            PassengerId = Guid.NewGuid().ToString(),
            BookingId = request.BookingId,
            Title = request.Title,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            PassportNumber = request.PassportNumber,
            Nationality = request.Nationality,
            PassportExpiry = request.PassportExpiry,
            SeatId = request.SeatId,
            SeatNumber = request.SeatNumber,
            TicketNumber = GenerateTicketNumber("AIR", "123"),
            PassengerType = request.PassengerType,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _repository.Add(passenger);
        return MapToDto(result);
    }

    public async Task<PassengerDto?> UpdatePassenger(string passengerId, CreatePassengerDto request)
    {
        if (!ValidatePassengerData(request))
            return null;

        var passenger = await _repository.FindByPassengerId(passengerId);
        if (passenger == null)
            return null;

        passenger.Title = request.Title;
        passenger.FirstName = request.FirstName;
        passenger.LastName = request.LastName;
        passenger.DateOfBirth = request.DateOfBirth;
        passenger.Gender = request.Gender;
        passenger.PassportNumber = request.PassportNumber;
        passenger.Nationality = request.Nationality;
        passenger.PassportExpiry = request.PassportExpiry;
        passenger.PassengerType = request.PassengerType;

        var result = await _repository.Update(passenger);
        return MapToDto(result);
    }

    public async Task<PassengerDto?> AssignSeat(string passengerId, string seatId, string seatNumber)
    {
        var passenger = await _repository.FindByPassengerId(passengerId);
        if (passenger == null)
            return null;

        passenger.SeatId = seatId;
        passenger.SeatNumber = seatNumber;

        var result = await _repository.Update(passenger);
        return MapToDto(result);
    }

    public async Task<bool> DeletePassenger(string passengerId)
    {
        return await _repository.Delete(passengerId);
    }

    public async Task<bool> DeletePassengersByBooking(string bookingId)
    {
        return await _repository.DeleteByBookingId(bookingId);
    }

    public string GenerateTicketNumber(string airlineCode, string flightNumber)
    {
        var random = new Random();
        var randomDigits = random.Next(100000, 999999).ToString();
        return $"{airlineCode}{flightNumber}-{randomDigits}";
    }

    public bool ValidatePassengerData(CreatePassengerDto request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
            return false;

        if (string.IsNullOrWhiteSpace(request.PassportNumber))
            return false;

        if (request.PassportExpiry <= DateTime.UtcNow)
            return false;

        var age = CalculateAge(request.DateOfBirth);
        if (age < 0)
            return false;

        return true;
    }

    private int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.UtcNow;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age))
            age--;
        return age;
    }

    private PassengerDto MapToDto(PassengerInfo passenger)
    {
        return new PassengerDto
        {
            PassengerId = passenger.PassengerId,
            BookingId = passenger.BookingId,
            Title = passenger.Title,
            FirstName = passenger.FirstName,
            LastName = passenger.LastName,
            DateOfBirth = passenger.DateOfBirth,
            Gender = passenger.Gender,
            PassportNumber = passenger.PassportNumber,
            Nationality = passenger.Nationality,
            PassportExpiry = passenger.PassportExpiry,
            SeatId = passenger.SeatId,
            SeatNumber = passenger.SeatNumber,
            TicketNumber = passenger.TicketNumber,
            PassengerType = passenger.PassengerType,
            CreatedAt = passenger.CreatedAt,
            UpdatedAt = passenger.UpdatedAt
        };
    }
}
