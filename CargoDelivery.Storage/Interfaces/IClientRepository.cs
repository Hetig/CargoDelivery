using CargoDelivery.Storage.Entities;

namespace CargoDelivery.Storage.Interfaces;

public interface IClientRepository
{
    Task<ClientDb> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<ClientDb>> GetAllAsync(CancellationToken cancellationToken);
    Task<ClientDb> AddAsync(ClientDb clientDb, CancellationToken cancellationToken);
}