namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель клиента для ответа
/// </summary>
public class ClientResponseDto
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Имя клиента
    /// </summary>
    public string Name { get; init; }
}