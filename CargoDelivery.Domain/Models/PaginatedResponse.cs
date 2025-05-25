namespace CargoDelivery.Domain.Models;

public class PaginatedResponse<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public required List<T> Data { get; set; }
}