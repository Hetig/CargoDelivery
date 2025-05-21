using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Enums;

namespace CargoDelivery.Domain.Models;

public class Order
{
    public Guid Id { get; init; }
    public DateTime CreateDateTime { get; init; }
    public OrderStatus Status { get; init; }
    public Client Client { get; init; }
    public Cargo Cargo { get; init; }
    public string TakeAddress { get; init; }
    public DateTime TakeDateTime { get; init; }
    public Courier Courier { get; init; }
    public string DestinationAddress { get; init; }
    public DateTime DestinationDateTime { get; init; }
    public bool Deleted { get; init; } = false;
    public string DeletedComment { get; init; }
}