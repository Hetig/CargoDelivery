using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.API.Dtos;

public class CargoCreateDto
{
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string Name { get; init; }
    
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string Description { get; init; }
}