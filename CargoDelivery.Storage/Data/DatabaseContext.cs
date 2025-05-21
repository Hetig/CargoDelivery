using CargoDelivery.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CargoDelivery.Storage.Data;

public class DatabaseContext : DbContext
{
    public DbSet<OrderDb> Orders => Set<OrderDb>();
    public DbSet<CargoDb> Cargos => Set<CargoDb>();
    public DbSet<ClientDb> Clients => Set<ClientDb>();
    public DbSet<CourierDb> Couriers => Set<CourierDb>();
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
}