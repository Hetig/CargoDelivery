using CargoDelivery.Storage.Enums;

namespace CargoDelivery.Storage.Entities;

public class OrderDb
{
    public Guid Id { get; set; }
    public DateTime CreateDateTime { get; set; }
    public OrderStatusDb Status { get; set; }
    public int StatusId { get; set; }
    public ClientDb Client { get; set; }
    public Guid ClientId { get; set; }
    public CargoDb Cargo { get; set; }
    public Guid CargoId { get; set; }
    public string TakeAddress { get; set; }
    public DateTime TakeDateTime { get; set; }
    public CourierDb? Courier { get; set; }
    public Guid? CourierId { get; set; }
    public string DestinationAddress { get; set; }
    public DateTime DestinationDateTime { get; set; }
    public bool Deleted { get; set; }
    public string? CancelledComment { get; set; }
}