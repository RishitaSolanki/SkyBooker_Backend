using AutoMapper;
using SkyBooker.FlightService.DTOs;
using SkyBooker.FlightService.Models;

namespace SkyBooker.FlightService.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Flight to FlightDto mapping
        CreateMap<Flight, FlightDto>()
            .ForMember(dest => dest.AirlineName, 
                opt => opt.MapFrom(src => src.Airline.AirlineName))
            .ForMember(dest => dest.Status, 
                opt => opt.MapFrom(src => src.Status.ToString()));

        // CreateFlightDto to Flight mapping
        CreateMap<CreateFlightDto, Flight>()
            .ForMember(dest => dest.FlightId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Airline, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.DurationMinutes, 
                opt => opt.MapFrom(src => CalculateDuration(src.DepartureTime, src.ArrivalTime)))
            .ForMember(dest => dest.AvailableSeats, 
                opt => opt.MapFrom(src => src.TotalSeats));

        // UpdateFlightDto to Flight mapping (only update non-null properties)
        CreateMap<UpdateFlightDto, Flight>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Airline mappings (if needed in future)
        CreateMap<Airline, AirlineDto>();

        // Airport mappings (if needed in future)
        CreateMap<Airport, AirportDto>();
    }

    // Helper method to calculate flight duration
    private int CalculateDuration(DateTime departure, DateTime arrival)
    {
        return (int)(arrival - departure).TotalMinutes;
    }
}

// Additional DTOs for Airline and Airport (optional - for future use)
public class AirlineDto
{
    public int AirlineId { get; set; }
    public string AirlineName { get; set; } = string.Empty;
    public string IATACode { get; set; } = string.Empty;
    public string? Country { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; }
}

public class AirportDto
{
    public int AirportId { get; set; }
    public string IATACode { get; set; } = string.Empty;
    public string AirportName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}