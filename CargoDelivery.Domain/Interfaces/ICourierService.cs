using CargoDelivery.Domain.Models;

namespace CargoDelivery.Domain.Interfaces;

public interface ICourierService
{
    Task<Courier> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Courier>> GetAllAsync(CancellationToken cancellationToken);
    Task<Courier> AddAsync(Courier client, CancellationToken cancellationToken);
}