namespace CargoDelivery.Storage.Entities;

public class CourierDb
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<OrderDb> Orders { get; set; }
}