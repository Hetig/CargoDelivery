using CargoDelivery.Storage.Enums;

namespace CargoDelivery.API.Dtos;

public class OrderResponseDto
{
    public Guid Id { get; init; }
    public DateTime CreateDateTime { get; init; }
    public OrderStatusDb StatusDb { get; init; }
    public ClientResponseDto Client { get; init; }
    public CargoResponseDto Cargo { get; init; }
    public string TakeAddress { get; init; }
    public DateTime TakeDateTime { get; init; }
    public CourierResponseDto Courier { get; init; }
    public string DestinationAddress { get; init; }
    public DateTime DestinationDateTime { get; init; }
    public bool Deleted { get; init; } = false;
    public string DeletedComment { get; init; }
}