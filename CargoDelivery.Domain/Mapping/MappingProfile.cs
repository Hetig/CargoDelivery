using AutoMapper;
using CargoDelivery.Domain.Models;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Enums;

namespace CargoDelivery.Domain.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OrderDb,Order>().ReverseMap();
        CreateMap<ClientDb, Client>().ReverseMap();
        CreateMap<CargoDb, Cargo>().ReverseMap();
        CreateMap<CourierDb, Courier>().ReverseMap();
        CreateMap<OrderStatus, OrderStatusDb>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (int)src))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ToString()))
            .ReverseMap();
    }
}