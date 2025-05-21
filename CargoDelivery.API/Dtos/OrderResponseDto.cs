using CargoDelivery.Storage.Enums;

namespace CargoDelivery.API.Dtos;

public class OrderResponseDto
{
    public Guid Id { get; init; }
    public DateTime CreateDateTime { get; init; }
    public OrderStatus Status { get; init; }
    public ClientDto Client { get; init; }
    public CargoDto Cargo { get; init; }
    public string TakeAddress { get; init; }
    public DateTime TakeDateTime { get; init; }
    public CourierDto Courier { get; init; }
    public string DestinationAddress { get; init; }
    public DateTime DestinationDateTime { get; init; }
    public bool Deleted { get; init; } = false;
    public string DeletedComment { get; init; }
}