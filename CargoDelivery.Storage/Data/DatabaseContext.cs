using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Enums;
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
        
        modelBuilder.Entity<CourierDb>().HasData(
            new CourierDb { Id = Guid.Parse("9BAB18DF-C059-414B-9E8C-7E3825C9EB28"), Name = "Courier1" },
            new CourierDb { Id = Guid.Parse("8A9DF209-331B-417B-87EB-FF2C2762B47C"), Name = "Courier2"  },
            new CourierDb { Id = Guid.Parse("DEAEDBB9-8E88-44A4-A639-8D6F5C43CA5F"), Name = "Courier3"  }
        );

        modelBuilder.Entity<ClientDb>().HasData(
            new ClientDb { Id = Guid.Parse("7C9A412F-409E-47E1-B2C0-93961F6F4853"), Name = "Client1"},    
            new ClientDb { Id = Guid.Parse("C9CD28CC-B080-4509-96C8-145049F7908C"), Name = "Client2" },    
            new ClientDb { Id = Guid.Parse("6B0DD6EF-FA49-4B24-8471-B7B74A10C5E6"), Name = "Client3" }    
        );

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
            .WithOne(cargo => cargo.Order) 
            .HasForeignKey<OrderDb>(ord => ord.CargoId) 
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}