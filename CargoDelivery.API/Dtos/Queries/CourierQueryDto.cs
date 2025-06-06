using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель курьера
/// </summary>
public class CourierQueryDto
{
    /// <summary>
    /// Идентификатор курьера
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Имя курьера
    /// </summary>
    public string Name { get; init; }
}