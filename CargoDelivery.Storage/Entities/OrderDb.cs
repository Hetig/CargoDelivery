using CargoDelivery.Storage.Enums;

namespace CargoDelivery.Storage.Entities;

public class OrderDb
{
    public Guid Id { get; init; }
    public DateTime CreateDateTime { get; init; }
    public OrderStatus Status { get; set; }
    public ClientDb Client { get; init; }
    public CargoDb Cargo { get; init; }
    public string TakeAddress { get; init; }
    public DateTime TakeDateTime { get; init; }
    public CourierDb? Courier { get; set; }
    public string DestinationAddress { get; init; }
    public DateTime DestinationDateTime { get; init; }
    public bool Deleted { get; set; }
    public string? CancelledComment { get; set; }

    public OrderDb()
    {
        CreateDateTime = DateTime.UtcNow;
        Status = OrderStatus.New;
    }
}