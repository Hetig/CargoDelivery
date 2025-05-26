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
    
    public async Task<(List<OrderDb> Data, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var totalCount = await _context.Orders.CountAsync(cancellationToken);

        var data = await _context.Orders
            .AsNoTracking()
            .Include(ord => ord.Cargo)   
            .Include(ord => ord.Client)  
            .Include(ord => ord.Courier)
            .OrderBy(o => o.CreateDateTime)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (data, totalCount);
    }
    
    public async Task<(List<OrderDb> Data, int TotalCount)> SearchAsync(string query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await GetPaginatedAsync(pageNumber, pageSize, cancellationToken);
        
        var data = await _context.Orders
                                .Where(ord => 
                                    ord.Client.Name.ToLower().Contains(query.ToLower()) ||
                                    ord.Cargo.Name.ToLower().Contains(query.ToLower()) ||
                                    ord.Courier.Name.ToLower().Contains(query.ToLower()) ||
                                    ord.DestinationAddress.ToLower().Contains(query.ToLower()) ||
                                    ord.TakeAddress.ToLower().Contains(query.ToLower()) ||
                                    ord.DestinationAddress.ToLower().Contains(query.ToLower()) ||
                                    ord.CreateDateTime.ToString().ToLower().Contains(query.ToLower()) ||
                                    ord.TakeDateTime.ToString().ToLower().Contains(query.ToLower()) ||
                                    ord.DestinationDateTime.ToString().ToLower().Contains(query.ToLower()))
                                .AsNoTracking()
                                .Include(ord => ord.Cargo)   
                                .Include(ord => ord.Client)  
                                .Include(ord => ord.Courier)
                                .ToListAsync(cancellationToken);
        
        return (data, data.Count);
    }

    public async Task<OrderDb> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders.Where(ord => ord.Deleted != true)
                                    .Include(ord => ord.Cargo)
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