using System.ComponentModel.DataAnnotations;
using CargoDelivery.API.ValidationAttributes;

namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель создания заказа
/// </summary>
public class OrderCreateDto : IValidatableObject
{
    [Required]
    public Guid ClientId { get; init; }
    [Required]
    public CargoCreateDto Cargo { get; init; }
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string TakeAddress { get; init; }
    [Required]
    [FutureDate(ErrorMessage = "Дата забора груза должна быть больше текущей даты и времени")]
    public DateTime TakeDateTime { get; init; }
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string DestinationAddress { get; init; }
    [Required]
    public DateTime DestinationDateTime { get; init; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DestinationDateTime <= TakeDateTime)
        {
            yield return new ValidationResult(
                "Дата доставки должна быть позже даты забора",
                new[] { nameof(DestinationDateTime) });
        }
    }
}