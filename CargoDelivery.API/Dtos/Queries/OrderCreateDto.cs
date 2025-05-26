using System.ComponentModel.DataAnnotations;
using CargoDelivery.API.ValidationAttributes;

namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель создания заказа
/// </summary>
public class OrderCreateDto : IValidatableObject
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    [Required]
    public Guid ClientId { get; init; }
    
    /// <summary>
    /// Груз
    /// </summary>
    [Required]
    public CargoCreateDto Cargo { get; init; }
    
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
    [FutureDate(ErrorMessage = "Дата забора груза должна быть больше текущей даты и времени")]
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