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
        CreateMap<OrderStatus, Domain.Enums.OrderStatus>().ReverseMap();
    }
}