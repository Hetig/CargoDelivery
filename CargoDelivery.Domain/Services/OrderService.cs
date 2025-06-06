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
    private readonly IClientService _clientService;

    public OrderService(IMapper mapper, IOrderRepository orderRepository, ICourierRepository courierRepository, IClientService clientService)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _courierRepository = courierRepository;
        _clientService = clientService;
    }
    
    public async Task<PaginatedResponse<Order>> GetAllPaginatedAsync(
        PaginationRequest request,
        CancellationToken cancellationToken)
    {
        var (data, totalCount) = await _orderRepository.GetPaginatedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var filteredData = data.Where(o => !o.Deleted).ToList();

        return new PaginatedResponse<Order>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            Data = _mapper.Map<List<Order>>(filteredData)
        };
    }
    public async Task<PaginatedResponse<Order>> SearchPaginatedAsync(
        string query, 
        PaginationRequest request,
        CancellationToken cancellationToken)
    {
        var (data, totalCount) = await _orderRepository.SearchAsync(
            query,
            request.PageNumber,
            request.PageSize,
            cancellationToken);
        
        var filteredData = data.Where(o => !o.Deleted).ToList();
        
        return new PaginatedResponse<Order>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            Data = _mapper.Map<List<Order>>(filteredData)
        };
    }

    public async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<Order>(order);
    }

    public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken)
    {
        var selectedClient = await _clientService.GetByIdAsync(order.ClientId, cancellationToken);
        if(selectedClient == null) return null;
        
        var createdOrder = await _orderRepository.AddAsync(_mapper.Map<OrderDb>(order), cancellationToken);
        
        return _mapper.Map<Order>(createdOrder);
    }
    
    public async Task<bool> UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(order.Id, cancellationToken);
        if (orderToUpdate == null || orderToUpdate?.Status != OrderStatus.New)
            return false;
        
        orderToUpdate.DestinationAddress = order.DestinationAddress;
        orderToUpdate.DestinationDateTime = order.DestinationDateTime;
        orderToUpdate.TakeAddress = order.TakeAddress;
        orderToUpdate.TakeDateTime = order.TakeDateTime;
        
        return await _orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
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
        if(orderInProcess == null) return false;
        
        orderInProcess.Status = OrderStatus.InProcess;
        
        return await _orderRepository.UpdateAsync(orderInProcess, cancellationToken);
    }

    public async Task<bool> SetDoneStatusAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var orderToDone = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if(orderToDone == null) return false;
        
        orderToDone.Status = OrderStatus.Done;
        
        return await _orderRepository.UpdateAsync(orderToDone, cancellationToken);
    }

    public async Task<bool> SetCancelStatusAsync(Guid orderId, string comment, CancellationToken cancellationToken)
    {
        var orderToCancel = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        if(orderToCancel == null) return false;
        
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