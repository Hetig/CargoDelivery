using CargoDelivery.Storage.Data;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CargoDelivery.Storage.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly DatabaseContext _dbContext;

    public ClientRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ClientDb> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Clients.FirstOrDefaultAsync(client => client.Id == id, cancellationToken);
    }

    public async Task<List<ClientDb>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Clients.AsNoTracking()
                                    .ToListAsync(cancellationToken);
    }

    public async Task<ClientDb> AddAsync(ClientDb clientDb, CancellationToken cancellationToken)
    {
        var createdClient = await _dbContext.Clients.AddAsync(clientDb, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return createdClient.Entity;
    }
}