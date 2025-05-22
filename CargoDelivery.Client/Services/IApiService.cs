using CargoDelivery.Domain.Models;
using Order = CargoDelivery.Client.Models.Order;

namespace CargoDelivery.Client.Services;

public interface IApiService
{
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Order> GetOrderByIdAsync(Guid id);
    Task<IEnumerable<Order>> SearchOrdersAsync(string query);
    Task<Order> CreateOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task OrderDoneAsync(Order order);
    Task OrderInProcessAsync(Order order);
    Task OrderCancelAsync(Order order);
    Task DeleteOrderAsync(Guid id);
        
    Task<IEnumerable<Courier>> GetCouriersAsync();
}