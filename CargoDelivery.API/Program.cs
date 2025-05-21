using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Mapping;
using CargoDelivery.Domain.Services;
using CargoDelivery.Storage.Data;
using CargoDelivery.Storage.Interfaces;
using CargoDelivery.Storage.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICourierRepository, CourierRepository>();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddAutoMapper(typeof(CargoDelivery.Domain.Mapping.MappingProfile));
builder.Services.AddAutoMapper(typeof(CargoDelivery.API.Mapping.MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
await context.Database.MigrateAsync();

app.Run();