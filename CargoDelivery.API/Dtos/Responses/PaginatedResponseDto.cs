namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель получения списка объектов
/// </summary>
/// <typeparam name="T"></typeparam>
public class PaginatedResponseDto<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public required List<T> Data { get; set; }
}