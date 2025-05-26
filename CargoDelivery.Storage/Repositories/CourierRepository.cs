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

    public async Task<List<CourierDb>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Couriers.AsNoTracking()
                                        .ToListAsync(cancellationToken);
    }

    public async Task<CourierDb> AddAsync(CourierDb courier, CancellationToken cancellationToken)
    {
        var createdCourier = await _dbContext.Couriers.AddAsync(courier, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return createdCourier.Entity;
    }
}