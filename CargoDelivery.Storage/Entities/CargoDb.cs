namespace CargoDelivery.Storage.Entities;

public class CargoDb
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public OrderDb Order { get; set; }
}