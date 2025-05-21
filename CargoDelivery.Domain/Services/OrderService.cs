using AutoMapper;
using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Models;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Interfaces;

namespace CargoDelivery.Domain.Services;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _repository;

    public OrderService(IMapper mapper, IOrderRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    
    public async Task<List<Order>> GetAllAsync()
    {
        var orders = await _repository.GetAllAsync();
        return _mapper.Map<List<Order>>(orders);
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        var order = await _repository.GetByIdAsync(id);
        return _mapper.Map<Order>(order);
    }

    public async Task<Order> AddAsync(Order order)
    {
        var createdOrder = await _repository.AddAsync(_mapper.Map<OrderDb>(order));
        return _mapper.Map<Order>(createdOrder);
    }
}