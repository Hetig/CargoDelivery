using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.Storage.Enums;

public class OrderStatusDb
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public static implicit operator OrderStatus(OrderStatusDb db) 
        => (OrderStatus)db.Id;
    
    public static implicit operator OrderStatusDb(OrderStatus status) 
        => new() { Id = (int)status, Name = status.ToString() };
}