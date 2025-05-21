using CargoDelivery.Domain.Models;

namespace CargoDelivery.Domain.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetAllAsync(CancellationToken cancellationToken);
    Task<Order> GetByIdAsync(Guid id,  CancellationToken cancellationToken);
    Task<Order> AddAsync(Order order,  CancellationToken cancellationToken);
}