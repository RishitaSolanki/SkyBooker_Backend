using Microsoft.EntityFrameworkCore;
using SkyBooker.BookingService.DTOs;
using SkyBooker.BookingService.Entities;
using SkyBooker.BookingService.Repositories;

namespace SkyBooker.BookingService.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingDto?> CreateBooking(CreateBookingDto request)
    {
        var fareSummary = await CalculateFare(request);
        var pnrCode = await GeneratePnr();

        var booking = new Booking
        {
            BookingId = Guid.NewGuid().ToString(),
            UserId = request.UserId,
            FlightId = request.FlightId,
            PnrCode = pnrCode,
            TripType = request.TripType,
            Status = "PENDING",
            BaseFare = fareSummary.BaseFare,
            Taxes = fareSummary.Taxes,
            TotalFare = fareSummary.TotalFare,
            MealPreference = request.MealPreference,
            LuggageKg = request.LuggageKg,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            BookedAt = DateTime.UtcNow
        };

        await _bookingRepository.Add(booking);
        await _bookingRepository.SaveChangesAsync();

        return MapToDto(booking);
    }

    public async Task<BookingDto?> GetBookingById(string bookingId)
    {
        var booking = await _bookingRepository.FindByBookingId(bookingId);
        return booking != null ? MapToDto(booking) : null;
    }

    public async Task<BookingDto?> GetBookingByPnr(string pnrCode)
    {
        var booking = await _bookingRepository.FindByPnrCode(pnrCode);
        return booking != null ? MapToDto(booking) : null;
    }

    public async Task<List<BookingDto>> GetBookingsByUser(string userId)
    {
        var bookings = await _bookingRepository.FindAllByUserId(userId);
        return bookings.Select(MapToDto).ToList();
    }

    public async Task<List<BookingDto>> GetBookingsByFlight(int flightId)
    {
        var bookings = await _bookingRepository.FindAllByFlightId(flightId);
        return bookings.Select(MapToDto).ToList();
    }

    public async Task<bool> CancelBooking(string bookingId)
    {
        var booking = await _bookingRepository.FindByBookingId(bookingId);
        if (booking == null) return false;

        booking.Status = "CANCELLED";
        await _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStatus(string bookingId, string status)
    {
        var booking = await _bookingRepository.FindByBookingId(bookingId);
        if (booking == null) return false;

        booking.Status = status;
        await _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync();
        return true;
    }

    public async Task<FareSummary> CalculateFare(CreateBookingDto request)
    {
        // Base fare calculation (simplified - in real system would fetch from FlightService)
        var baseFare = 5000m; // Default base fare
        var taxes = baseFare * 0.18m; // 18% GST
        var ancillaryCost = request.LuggageKg * 100m; // 100 per kg extra luggage
        var totalFare = baseFare + taxes + ancillaryCost;

        return new FareSummary(baseFare, taxes, ancillaryCost, totalFare);
    }

    public async Task<string> GeneratePnr()
    {
        // Generate 6-character alphanumeric PNR
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        string pnr;
        bool exists;

        do
        {
            pnr = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            exists = await _bookingRepository.FindByPnrCode(pnr) != null;
        } while (exists);

        return pnr;
    }

    public async Task<List<BookingDto>> GetUpcomingBookings(string userId)
    {
        var bookings = await _bookingRepository.FindAllByUserId(userId);
        return bookings
            .Where(b => b.Status == "PENDING" || b.Status == "CONFIRMED")
            .Select(MapToDto)
            .ToList();
    }

    private BookingDto MapToDto(Booking booking)
    {
        return new BookingDto
        {
            BookingId = booking.BookingId,
            UserId = booking.UserId,
            FlightId = booking.FlightId,
            PnrCode = booking.PnrCode,
            TripType = booking.TripType,
            Status = booking.Status,
            BaseFare = booking.BaseFare,
            Taxes = booking.Taxes,
            TotalFare = booking.TotalFare,
            MealPreference = booking.MealPreference,
            LuggageKg = booking.LuggageKg,
            ContactEmail = booking.ContactEmail,
            ContactPhone = booking.ContactPhone,
            BookedAt = booking.BookedAt,
            PaymentId = booking.PaymentId
        };
    }
}
