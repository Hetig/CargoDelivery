using System.ComponentModel.DataAnnotations;
using CargoDelivery.API.ValidationAttributes;

namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель обновления данных заказа
/// </summary>
public class OrderUpdateDto : IValidatableObject
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    [Required]
    public Guid Id { get; init; }
    
    /// <summary>
    /// Адрес погрузки
    /// </summary>
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string TakeAddress { get; init; }
    
    /// <summary>
    /// Дата и время погрузки
    /// </summary>
    [Required]
    public DateTime TakeDateTime { get; init; }
    
    /// <summary>
    /// Адрес доставки
    /// </summary>
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string DestinationAddress { get; init; }
    
    /// <summary>
    /// Дата и время доставки
    /// </summary>
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