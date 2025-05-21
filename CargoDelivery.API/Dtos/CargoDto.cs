using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.API.Dtos;

public class CargoDto
{
    public Guid Id { get; init; }
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string Name { get; init; }
}