using CargoDelivery.Client.Models;
using CargoDelivery.Client.Models.Queries;

namespace CargoDelivery.Client.Services;

public interface IApiService
{
    Task<PaginatedResponse<Order>> GetOrdersAsync(int pageNumber = 1, int pageSize = 10);
    Task<Order> GetOrderByIdAsync(Guid id);
    Task<Order> CreateOrderAsync(Order order);
    Task<bool> UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(Guid id);
    Task<bool> AssignToCourierAsync(Guid orderId, Guid courierId);
    Task<bool> SetInProcessAsync(Guid orderId);
    Task<bool> SetDoneAsync(Guid orderId);
    Task<bool> SetCancelAsync(Guid orderId, string comment);
    Task<List<Models.Client>> GetClientsAsync();
    Task<List<Courier>> GetCouriersAsync();
    Task<Models.Client> CreateClientAsync(CreateClient client);
    Task<Courier> CreateCourierAsync(CreateCourier courier);
    
}