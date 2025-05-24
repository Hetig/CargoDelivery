namespace CargoDelivery.Storage.Entities;

public class ClientDb
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public List<OrderDb> Orders { get; init; }
}