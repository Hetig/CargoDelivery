using CargoDelivery.Storage.Data;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CargoDelivery.Storage.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DatabaseContext _context;

    public OrderRepository(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<List<OrderDb>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Orders
                                .AsNoTracking()
                                .Include(ord => ord.Cargo)   
                                .Include(ord => ord.Client)  
                                .Include(ord => ord.Courier)
                                .ToListAsync(cancellationToken);
    }

    public async Task<OrderDb> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders.Include(ord => ord.Cargo)
                                    .Include(ord => ord.Client)
                                    .Include(ord => ord.Courier)
                                        .FirstOrDefaultAsync(ord => ord.Id == id, cancellationToken);
    }

    public async Task<OrderDb> AddAsync(OrderDb orderDb, CancellationToken cancellationToken = default)
    { 
        var createdOrder = await _context.Orders.AddAsync(orderDb, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return createdOrder.Entity;
    }
}