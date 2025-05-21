using CargoDelivery.Storage.Entities;

namespace CargoDelivery.Storage.Interfaces;

public interface IOrderRepository
{ 
    Task<List<OrderDb>> GetAllAsync();
    Task<OrderDb> GetByIdAsync(Guid id);
    Task<OrderDb> AddAsync(OrderDb orderDb);
}