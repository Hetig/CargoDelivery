using AutoMapper;
using CargoDelivery.API.Dtos;
using CargoDelivery.Domain.Models;
using CargoDelivery.Storage.Entities;

namespace CargoDelivery.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OrderCreateDto, Order>().ReverseMap();
        CreateMap<OrderUpdateDto, Order>().ReverseMap();
        CreateMap<OrderResponseDto, Order>().ReverseMap();
        
        CreateMap<CargoDto, Cargo>().ReverseMap();
        CreateMap<ClientDto, Client>().ReverseMap();
        CreateMap<CourierDto, Courier>().ReverseMap();
    }
}