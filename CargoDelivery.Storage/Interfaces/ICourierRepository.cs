using CargoDelivery.Storage.Entities;

namespace CargoDelivery.Storage.Interfaces;

public interface ICourierRepository
{
    Task<CourierDb> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<CourierDb>> GetAllAsync(CancellationToken cancellationToken);
    Task<CourierDb> AddAsync(CourierDb courier, CancellationToken cancellationToken);
}
