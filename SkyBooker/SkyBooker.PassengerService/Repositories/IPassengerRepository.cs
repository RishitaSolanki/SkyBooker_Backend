using SkyBooker.PassengerService.Entities;

namespace SkyBooker.PassengerService.Repositories;

public interface IPassengerRepository
{
    Task<PassengerInfo?> FindByPassengerId(string passengerId);
    Task<PassengerInfo?> FindByBookingId(string bookingId);
    Task<PassengerInfo?> FindByPassportNumber(string passportNumber);
    Task<PassengerInfo?> FindByTicketNumber(string ticketNumber);
    Task<PassengerInfo?> FindBySeatId(string seatId);
    Task<List<PassengerInfo>> FindByBookingIdList(string bookingId);
    Task<int> CountByBookingId(string bookingId);
    Task<bool> DeleteByBookingId(string bookingId);
    Task<PassengerInfo> Add(PassengerInfo passenger);
    Task<PassengerInfo> Update(PassengerInfo passenger);
    Task<bool> Delete(string passengerId);
}
