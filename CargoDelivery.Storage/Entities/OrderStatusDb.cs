using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CargoDelivery.Storage.Entities;

namespace CargoDelivery.Storage.Enums;

public class OrderStatusDb
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<OrderDb> Orders { get; set; }
}