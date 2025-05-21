using CargoDelivery.Storage.Entities;

namespace CargoDelivery.Storage.Interfaces;

public interface IOrderRepository
{ 
    Task<List<OrderDb>> GetAllAsync(CancellationToken cancellationToken);
    Task<OrderDb> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<OrderDb> AddAsync(OrderDb orderDb, CancellationToken cancellationToken);
}