using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.API.Dtos;

public class ClientQueryDto
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string Name { get; init; }
}