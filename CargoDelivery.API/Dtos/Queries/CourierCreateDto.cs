using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель создания курьера
/// </summary>
public class CourierCreateDto
{
    /// <summary>
    /// Имя курьера
    /// </summary>
    [Required]
    public string Name { get; init; }
}