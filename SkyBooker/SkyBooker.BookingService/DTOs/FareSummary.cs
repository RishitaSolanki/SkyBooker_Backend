namespace SkyBooker.BookingService.DTOs;

public record FareSummary(decimal BaseFare, decimal Taxes, decimal AncillaryCost, decimal TotalFare);
