using CargoDelivery.Domain.Models;
using CargoDelivery.Storage.Enums;

namespace CargoDelivery.Domain.Interfaces;

public interface IOrderService
{
    Task<bool> UpdateAsync( Order order, CancellationToken cancellationToken);
    Task<PaginatedResponse<Order>> SearchPaginatedAsync(string query, PaginationRequest request, CancellationToken cancellationToken);
    Task<Order> GetByIdAsync(Guid id,  CancellationToken cancellationToken);
    Task<Order> AddAsync(Order order,  CancellationToken cancellationToken);
    Task<bool> AssignToCourierAsync(Guid courierId, Guid orderId, CancellationToken cancellationToken);
    Task<bool> SetInProcessStatusAsync(Guid orderId, CancellationToken cancellationToken);
    Task<bool> SetDoneStatusAsync(Guid orderId, CancellationToken cancellationToken);
    Task<bool> SetCancelStatusAsync(Guid orderId, string comment, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid orderId, CancellationToken cancellationToken);
    Task<PaginatedResponse<Order>> GetAllPaginatedAsync(PaginationRequest request, CancellationToken cancellationToken);
}