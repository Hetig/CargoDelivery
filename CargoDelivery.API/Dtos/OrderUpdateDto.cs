using System.ComponentModel.DataAnnotations;
using CargoDelivery.API.ValidationAttributes;

namespace CargoDelivery.API.Dtos;

public class OrderUpdateDto
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    public ClientDto Client { get; init; }
    [Required]
    public CargoDto Cargo { get; init; }
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string TakeAddress { get; init; }
    [Required]
    public DateTime TakeDateTime { get; init; }
    [Required]
    public CourierDto Courier { get; init; }
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string DestinationAddress { get; init; }
    [Required]
    public DateTime DestinationDateTime { get; init; }
}