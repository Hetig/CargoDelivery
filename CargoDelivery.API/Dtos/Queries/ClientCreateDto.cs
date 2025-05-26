using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель создания клиента
/// </summary>
public class ClientCreateDto
{
    /// <summary>
    /// Имя клиента
    /// </summary>
    [Required]
    public string Name { get; init; }
}