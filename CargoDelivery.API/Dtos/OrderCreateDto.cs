using System.ComponentModel.DataAnnotations;
using CargoDelivery.API.ValidationAttributes;

namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель создания заказа
/// </summary>
public class OrderCreateDto
{
    [Required]
    public ClientDto Client { get; init; }
    [Required]
    public CargoDto Cargo { get; init; }
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string TakeAddress { get; init; }
    [Required]
    [FutureDate(ErrorMessage = "Event date must be in the future")]
    public DateTime TakeDateTime { get; init; }
    public CourierDto? Courier { get; init; }
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string DestinationAddress { get; init; }
    [Required]
    [FutureDate(ErrorMessage = "Event date must be in the future")]
    public DateTime DestinationDateTime { get; init; }
}