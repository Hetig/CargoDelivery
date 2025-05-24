using System.ComponentModel.DataAnnotations;
using CargoDelivery.API.ValidationAttributes;

namespace CargoDelivery.API.Dtos;

public class OrderUpdateDto
{
    [Required]
    public Guid Id { get; init; }
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string TakeAddress { get; init; }
    [Required]
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