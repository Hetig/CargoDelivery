using AutoMapper;
using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Models;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Enums;
using CargoDelivery.Storage.Interfaces;

namespace CargoDelivery.Domain.Services;

public class OrderService : IOrderService
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly ICourierRepository _courierRepository;

    public OrderService(IMapper mapper, IOrderRepository orderRepository, ICourierRepository courierRepository)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _courierRepository = courierRepository;
    }
    
    public async Task<List<Order>> GetAllAsync(CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<Order>>(orders);
    }
    public async Task<List<Order>> SearchAsync(string query, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.SearchAsync(query, cancellationToken);
        return _mapper.Map<List<Order>>(orders);
    }

    public async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<Order>(order);
    }

    public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken)
    {
        var createdOrder = await _orderRepository.AddAsync(_mapper.Map<OrderDb>(order), cancellationToken);
        
        return _mapper.Map<Order>(createdOrder);
    }
    
    public async Task<bool> UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(order.Id, cancellationToken);
        if (orderToUpdate?.Status != OrderStatus.New)
            return false;
        
        var createdOrder = await _orderRepository.UpdateAsync(_mapper.Map(order, orderToUpdate), cancellationToken);
        return true;
    }

    public async Task<bool> AssignToCourierAsync(Guid courierId, Guid orderId, CancellationToken cancellationToken)
    {
        var assignedOrder = await _orderRepository.GetByIdAsync(orderId, cancellationToken);

        if (assignedOrder.Courier != null) return false;
        
        assignedOrder.Courier = await _courierRepository.GetByIdAsync(courierId, cancellationToken);
        return await _orderRepository.UpdateAsync(assignedOrder, cancellationToken);
    }

    public async Task<bool> SetInProcessStatusAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var orderInProcess = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        orderInProcess.Status = OrderStatus.InProcess;
        
        return await _orderRepository.UpdateAsync(orderInProcess, cancellationToken);
    }

    public async Task<bool> SetDoneStatusAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var orderToDone = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        orderToDone.Status = OrderStatus.Done;
        
        return await _orderRepository.UpdateAsync(orderToDone, cancellationToken);
    }

    public async Task<bool> SetCancelStatusAsync(Guid orderId, string comment, CancellationToken cancellationToken)
    {
        var orderToCancel = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        orderToCancel.Status = OrderStatus.Cancelled;
        orderToCancel.CancelledComment = comment;
        
        return await _orderRepository.UpdateAsync(orderToCancel, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var orderToDelete = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        orderToDelete.Deleted = true;
        
        return await _orderRepository.UpdateAsync(orderToDelete, cancellationToken);
    }
}