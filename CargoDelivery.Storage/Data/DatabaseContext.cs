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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderDb>()
            .HasOne(ord => ord.Client)
            .WithMany(cl => cl.Orders)
            .HasForeignKey(ord => ord.ClientId) 
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<OrderDb>()
            .HasOne(ord => ord.Courier)
            .WithMany(co => co.Orders)
            .HasForeignKey(ord => ord.CourierId) 
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderDb>()
            .HasOne(ord => ord.Cargo)
            .WithOne() 
            .HasForeignKey<OrderDb>(ord => ord.CargoId) 
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}