using CargoDelivery.Storage.Data;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CargoDelivery.Storage.Repositories;

public class CourierRepository : ICourierRepository
{
    private readonly DatabaseContext _dbContext;

    public CourierRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<CourierDb> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Couriers.FirstOrDefaultAsync(courier => courier.Id == id, cancellationToken);
    }
}