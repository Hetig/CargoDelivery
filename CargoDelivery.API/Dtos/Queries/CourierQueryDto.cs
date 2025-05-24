using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.API.Dtos;

public class CourierQueryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}