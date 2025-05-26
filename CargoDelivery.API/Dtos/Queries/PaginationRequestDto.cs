namespace CargoDelivery.API.Dtos;

/// <summary>
/// Модель для получения части списка
/// </summary>
/// <param name="PageNumber">Номер страницы</param>
/// <param name="PageSize">Размер страницы</param>
public record PaginationRequestDto(
    int PageNumber = 1,
    int PageSize = 10);