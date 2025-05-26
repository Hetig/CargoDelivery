using System.ComponentModel.DataAnnotations;

namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель клиента
/// </summary>
public class ClientQueryDto
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    [Required]
    public Guid Id { get; init; }
    
    /// <summary>
    /// Имя клиента
    /// </summary>
    [Required]
    [StringLength(150, MinimumLength = 3)] 
    public string Name { get; init; }
}