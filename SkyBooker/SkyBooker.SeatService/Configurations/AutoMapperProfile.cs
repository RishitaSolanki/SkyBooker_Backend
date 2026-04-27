using AutoMapper;
using SkyBooker.SeatService.DTOs;
using SkyBooker.SeatService.Entities;

namespace SkyBooker.SeatService.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Seat, SeatDto>();
        CreateMap<CreateSeatDto, Seat>();
        CreateMap<UpdateSeatDto, Seat>();
    }
}
