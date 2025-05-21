using CargoDelivery.Domain.Models;

namespace CargoDelivery.Domain.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetAllAsync();
    Task<Order> GetByIdAsync(Guid id);
    Task<Order> AddAsync(Order order);
}