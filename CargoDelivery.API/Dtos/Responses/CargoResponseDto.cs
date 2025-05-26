namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель груза для ответа
/// </summary>
public class CargoResponseDto
{
    /// <summary>
    /// Идентификатор груза
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Наименование груза
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// Описание груза
    /// </summary>
    public string Description { get; init; }
}