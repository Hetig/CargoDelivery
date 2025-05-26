using CargoDelivery.Domain.Models;

namespace CargoDelivery.Domain.Interfaces;

public interface IClientService
{
    Task<Client> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Client>> GetAllAsync(CancellationToken cancellationToken);
    Task<Client> AddAsync(Client client, CancellationToken cancellationToken);
}