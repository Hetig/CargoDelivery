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
        
        CreateMap<CargoCreateDto, Cargo>().ReverseMap();
        CreateMap<CargoResponseDto, Cargo>().ReverseMap();
        
        CreateMap<ClientQueryDto, Client>().ReverseMap();
        CreateMap<ClientResponseDto, Client>().ReverseMap();
        
        CreateMap<CourierQueryDto, Courier>().ReverseMap();
        CreateMap<CourierResponseDto, Courier>().ReverseMap();

        CreateMap<PaginationRequestDto, PaginationRequest>().ReverseMap();
    }
}