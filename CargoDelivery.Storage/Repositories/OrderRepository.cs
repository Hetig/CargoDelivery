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
    public async Task<List<OrderDb>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await GetAllAsync();
        
        return await _context.Orders
                                .Where(ord => 
                                    ord.Client.Name.ToLower().Contains(query.ToLower()) ||
                                    ord.Cargo.Name.ToLower().Contains(query.ToLower()) ||
                                    ord.Courier.Name.ToLower().Contains(query.ToLower()) ||
                                    ord.DestinationAddress.ToLower().Contains(query.ToLower()) ||
                                    ord.TakeAddress.ToLower().Contains(query.ToLower()) ||
                                    ord.DestinationAddress.ToLower().Contains(query.ToLower()))
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

    public async Task<bool> UpdateAsync(OrderDb orderDb, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Entry(orderDb).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}