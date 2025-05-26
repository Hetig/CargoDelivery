using CargoDelivery.Client.Enums;
using CargoDelivery.Client.Models;

namespace CargoDelivery.Client.Services;

public interface IApiService
{
    Task<PaginatedResponse<Order>> GetOrdersAsync(int pageNumber = 1, int pageSize = 10);
    Task<Order> GetOrderByIdAsync(Guid id);
    Task<Order> CreateOrderAsync(Order order);
    Task<bool> UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(Guid id);
    Task<bool> AssignToCourierAsync(Guid orderId, Guid courierId);
    Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status, string comment = null);
}