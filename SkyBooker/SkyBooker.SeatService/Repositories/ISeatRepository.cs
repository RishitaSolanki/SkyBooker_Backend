using SkyBooker.SeatService.Entities;

namespace SkyBooker.SeatService.Repositories;

public interface ISeatRepository
{
    Task<Seat?> FindBySeatId(int seatId);
    Task<Seat?> FindByFlightIdAndSeatNumber(int flightId, string seatNumber);
    Task<List<Seat>> FindByFlightId(int flightId);
    Task<List<Seat>> FindByFlightIdAndSeatClass(int flightId, string seatClass);
    Task<List<Seat>> FindAvailableByFlightId(int flightId);
    Task<List<Seat>> FindAvailableByClass(int flightId, string seatClass);
    Task<int> CountAvailableByClass(int flightId, string seatClass);
    Task AddSeat(Seat seat);
    Task UpdateSeat(Seat seat);
    Task DeleteSeat(int seatId);
    Task DeleteSeatsForFlight(int flightId);
    Task SaveChangesAsync();
}
