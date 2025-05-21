using CargoDelivery.Storage.Entities;

namespace CargoDelivery.Storage.Interfaces;

public interface ICourierRepository
{
    Task<CourierDb> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
