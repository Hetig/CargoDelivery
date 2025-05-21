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
    
    public async Task<List<Order>> GetAllAsync(CancellationToken cancellationToken)
    {
        var orders = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<Order>>(orders);
    }

    public async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<Order>(order);
    }

    public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken)
    {
        var createdOrder = await _repository.AddAsync(_mapper.Map<OrderDb>(order), cancellationToken);
        return _mapper.Map<Order>(createdOrder);
    }
}