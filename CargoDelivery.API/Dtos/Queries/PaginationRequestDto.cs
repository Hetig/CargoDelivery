namespace CargoDelivery.API.Dtos;

public record PaginationRequestDto(
    int PageNumber = 1,
    int PageSize = 10);