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
    
    public async Task<List<OrderDb>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<OrderDb> GetByIdAsync(Guid id)
    {
        return await _context.Orders.FirstOrDefaultAsync(ord => ord.Id == id);
    }

    public async Task<OrderDb> AddAsync(OrderDb orderDb)
    { 
        var createdOrder = await _context.Orders.AddAsync(orderDb);
        await _context.SaveChangesAsync();
        return createdOrder.Entity;
    }
}