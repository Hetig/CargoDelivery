using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель создания груза
/// </summary>
public class CargoCreateDto
{
    /// <summary>
    /// Название груза
    /// </summary>
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string Name { get; init; }
    
    /// <summary>
    /// Описание груза
    /// </summary>
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string Description { get; init; }
}