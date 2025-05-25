using CargoDelivery.Storage.Entities;

namespace CargoDelivery.Storage.Interfaces;

public interface IOrderRepository
{ 
    Task<(List<OrderDb> Data, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<(List<OrderDb> Data, int TotalCount)> SearchAsync(string query, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<OrderDb> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<OrderDb> AddAsync(OrderDb orderDb, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(OrderDb orderDb, CancellationToken cancellationToken);
}