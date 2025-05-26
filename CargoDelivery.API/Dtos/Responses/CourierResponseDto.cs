namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель курьера для ответа
/// </summary>
public class CourierResponseDto
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