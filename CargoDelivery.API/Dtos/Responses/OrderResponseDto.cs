using CargoDelivery.Storage.Enums;

namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель заказа для ответа
/// </summary>
public class OrderResponseDto
{
    /// <summary>
    /// Идентификатор заказа
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Дата и время создания заказа
    /// </summary>
    public DateTime CreateDateTime { get; init; }
    
    /// <summary>
    /// Статус заказа
    /// </summary>
    public OrderStatus Status { get; init; }
    
    /// <summary>
    /// Клиент сделавший заказ
    /// </summary>
    public ClientResponseDto Client { get; init; }
    
    /// <summary>
    /// Груз
    /// </summary>
    public CargoResponseDto Cargo { get; init; }
    
    /// <summary>
    /// Адрес погрузки
    /// </summary>
    public string TakeAddress { get; init; }
    
    /// <summary>
    /// Дата и время погрузки
    /// </summary>
    public DateTime TakeDateTime { get; init; }
    
    /// <summary>
    /// Курьер
    /// </summary>
    public CourierResponseDto Courier { get; init; }
    
    /// <summary>
    /// Адрес доставки
    /// </summary>
    public string DestinationAddress { get; init; }
    
    /// <summary>
    /// Дата и время доставки
    /// </summary>
    public DateTime DestinationDateTime { get; init; }
    
    /// <summary>
    /// Комментарий при удалении
    /// </summary>
    public string CancelledComment { get; init; }
}