using CargoDelivery.Storage.Enums;

namespace CargoDelivery.Storage.Entities;

public class OrderDb
{
    public Guid Id { get; init; }
    public DateTime CreateDateTime { get; init; }
    public OrderStatus Status { get; init; }
    public ClientDb Client { get; init; }
    public CargoDb Cargo { get; init; }
    public string TakeAddress { get; init; }
    public DateTime TakeDateTime { get; init; }
    public CourierDb Courier { get; init; }
    public string DestinationAddress { get; init; }
    public DateTime DestinationDateTime { get; init; }
    public bool Deleted { get; init; } = false;
    public string? DeletedComment { get; init; }

    public OrderDb()
    {
        CreateDateTime = DateTime.UtcNow;
        Status = OrderStatus.New;
    }
}