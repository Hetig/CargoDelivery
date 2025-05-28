using AutoMapper;
using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Models;
using CargoDelivery.Domain.Services;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Enums;
using CargoDelivery.Storage.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CargoDelivery.Tests;

public class OrderServiceTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<ICourierRepository> _courierRepositoryMock = new();
    private readonly Mock<IClientService> _clientServiceMock = new();
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _orderService = new OrderService(
            _mapperMock.Object,
            _orderRepositoryMock.Object,
            _courierRepositoryMock.Object,
            _clientServiceMock.Object);
    }

    [Fact]
    public async Task GetAllPaginatedAsync_ReturnsPaginatedResponse()
    {
        var request = new PaginationRequest { PageNumber = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;
        
        var dbOrders = new List<OrderDb>
        {
            new() { Id = Guid.NewGuid(), Deleted = false },
            new() { Id = Guid.NewGuid(), Deleted = false },
            new() { Id = Guid.NewGuid(), Deleted = true } 
        };
        
        var expectedOrders = new List<Order>
        {
            new() { Id = dbOrders[0].Id },
            new() { Id = dbOrders[1].Id }
        };

        _orderRepositoryMock.Setup(x => x.GetPaginatedAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken))
            .ReturnsAsync((dbOrders, 2));
        
        _mapperMock.Setup(x => x.Map<List<Order>>(It.Is<List<OrderDb>>(l => l.Count == 2)))
            .Returns(expectedOrders);

        var result = await _orderService.GetAllPaginatedAsync(request, cancellationToken);

        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(request.PageNumber);
        result.PageSize.Should().Be(request.PageSize);
    }

    [Fact]
    public async Task SearchPaginatedAsync_ReturnsFilteredResults()
    {
        var query = "test";
        var request = new PaginationRequest { PageNumber = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;
        
        var dbOrders = new List<OrderDb>
        {
            new() { Id = Guid.NewGuid(), Deleted = false },
            new() { Id = Guid.NewGuid(), Deleted = false }
        };
        
        var expectedOrders = new List<Order>
        {
            new() { Id = dbOrders[0].Id },
            new() { Id = dbOrders[1].Id }
        };

        _orderRepositoryMock.Setup(x => x.SearchAsync(
                query,
                request.PageNumber,
                request.PageSize,
                cancellationToken))
            .ReturnsAsync((dbOrders, 2));
        
        _mapperMock.Setup(x => x.Map<List<Order>>(It.IsAny<List<OrderDb>>()))
            .Returns(expectedOrders);

        var result = await _orderService.SearchPaginatedAsync(query, request, cancellationToken);

        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOrder_WhenExists()
    {
        var orderId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        
        var dbOrder = new OrderDb { Id = orderId };
        var expectedOrder = new Order { Id = orderId };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync(dbOrder);
        
        _mapperMock.Setup(x => x.Map<Order>(dbOrder))
            .Returns(expectedOrder);

        var result = await _orderService.GetByIdAsync(orderId, cancellationToken);

        result.Should().NotBeNull();
        result.Id.Should().Be(orderId);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        var orderId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync((OrderDb?)null);

        var result = await _orderService.GetByIdAsync(orderId, cancellationToken);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ReturnsOrder_WhenClientExists()
    {
        var order = new Order { ClientId = Guid.NewGuid() };
        var cancellationToken = CancellationToken.None;
        
        var dbOrder = new OrderDb { Id = Guid.NewGuid(), ClientId = order.ClientId };
        var expectedOrder = new Order { Id = dbOrder.Id, ClientId = order.ClientId };

        _clientServiceMock.Setup(x => x.GetByIdAsync(order.ClientId, cancellationToken))
            .ReturnsAsync(new Client()); 
            
        _orderRepositoryMock.Setup(x => x.AddAsync(It.IsAny<OrderDb>(), cancellationToken))
            .ReturnsAsync(dbOrder);
        
        _mapperMock.Setup(x => x.Map<OrderDb>(order))
            .Returns(dbOrder);
            
        _mapperMock.Setup(x => x.Map<Order>(dbOrder))
            .Returns(expectedOrder);

        var result = await _orderService.AddAsync(order, cancellationToken);

        result.Should().NotBeNull();
        result.Id.Should().Be(dbOrder.Id);
        result.ClientId.Should().Be(order.ClientId);
    }

    [Fact]
    public async Task AddAsync_ReturnsNull_WhenClientNotExists()
    {
        var order = new Order { ClientId = Guid.NewGuid() };
        var cancellationToken = CancellationToken.None;

        _clientServiceMock.Setup(x => x.GetByIdAsync(order.ClientId, cancellationToken))
            .ReturnsAsync((Client?)null);

        var result = await _orderService.AddAsync(order, cancellationToken);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ReturnsTrue_WhenOrderExistsAndStatusNew()
    {
        var orderId = Guid.NewGuid();
        var order = new Order 
        { 
            Id = orderId,
            DestinationAddress = "New Address",
            TakeAddress = "New Take Address",
            DestinationDateTime = DateTime.Now.AddDays(1),
            TakeDateTime = DateTime.Now
        };
        
        var cancellationToken = CancellationToken.None;
        
        var dbOrder = new OrderDb 
        { 
            Id = orderId,
            Status = OrderStatus.New,
            DestinationAddress = "Old Address",
            TakeAddress = "Old Take Address"
        };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync(dbOrder);
            
        _orderRepositoryMock.Setup(x => x.UpdateAsync(dbOrder, cancellationToken))
            .ReturnsAsync(true);

        var result = await _orderService.UpdateAsync(order, cancellationToken);

        result.Should().BeTrue();
        dbOrder.DestinationAddress.Should().Be(order.DestinationAddress);
        dbOrder.TakeAddress.Should().Be(order.TakeAddress);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenOrderNotExists()
    {
        var order = new Order { Id = Guid.NewGuid() };
        var cancellationToken = CancellationToken.None;

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(order.Id, cancellationToken))
            .ReturnsAsync((OrderDb?)null);

        var result = await _orderService.UpdateAsync(order, cancellationToken);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenOrderStatusNotNew()
    {
        var orderId = Guid.NewGuid();
        var order = new Order { Id = orderId };
        var cancellationToken = CancellationToken.None;
        
        var dbOrder = new OrderDb { Id = orderId, Status = OrderStatus.InProcess };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync(dbOrder);

        var result = await _orderService.UpdateAsync(order, cancellationToken);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task AssignToCourierAsync_ReturnsTrue_WhenOrderAndCourierExistAndNoCourierAssigned()
    {
        var orderId = Guid.NewGuid();
        var courierId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        
        var dbOrder = new OrderDb { Id = orderId, Courier = null };
        var dbCourier = new CourierDb { Id = courierId };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync(dbOrder);
            
        _courierRepositoryMock.Setup(x => x.GetByIdAsync(courierId, cancellationToken))
            .ReturnsAsync(dbCourier);
            
        _orderRepositoryMock.Setup(x => x.UpdateAsync(dbOrder, cancellationToken))
            .ReturnsAsync(true);

        var result = await _orderService.AssignToCourierAsync(courierId, orderId, cancellationToken);

        result.Should().BeTrue();
        dbOrder.Courier.Should().Be(dbCourier);
    }

    [Fact]
    public async Task AssignToCourierAsync_ReturnsFalse_WhenCourierAlreadyAssigned()
    {
        var orderId = Guid.NewGuid();
        var courierId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        
        var dbOrder = new OrderDb 
        { 
            Id = orderId, 
            Courier = new CourierDb { Id = Guid.NewGuid() } 
        };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync(dbOrder);

        var result = await _orderService.AssignToCourierAsync(courierId, orderId, cancellationToken);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task SetInProcessStatusAsync_ReturnsTrue_WhenOrderExists()
    {
        var orderId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        
        var dbOrder = new OrderDb { Id = orderId, Status = OrderStatus.New };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync(dbOrder);
            
        _orderRepositoryMock.Setup(x => x.UpdateAsync(dbOrder, cancellationToken))
            .ReturnsAsync(true);

        var result = await _orderService.SetInProcessStatusAsync(orderId, cancellationToken);

        result.Should().BeTrue();
        dbOrder.Status.Should().Be(OrderStatus.InProcess);
    }

    [Fact]
    public async Task SetInProcessStatusAsync_ReturnsFalse_WhenOrderNotExists()
    {
        var orderId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync((OrderDb?)null);

        var result = await _orderService.SetInProcessStatusAsync(orderId, cancellationToken);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task SetDoneStatusAsync_ReturnsTrue_WhenOrderExists()
    {
        var orderId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        
        var dbOrder = new OrderDb { Id = orderId, Status = OrderStatus.InProcess };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync(dbOrder);
            
        _orderRepositoryMock.Setup(x => x.UpdateAsync(dbOrder, cancellationToken))
            .ReturnsAsync(true);

        var result = await _orderService.SetDoneStatusAsync(orderId, cancellationToken);

        result.Should().BeTrue();
        dbOrder.Status.Should().Be(OrderStatus.Done);
    }

    [Fact]
    public async Task SetCancelStatusAsync_ReturnsTrue_WhenOrderExists()
    {
        var orderId = Guid.NewGuid();
        var comment = "Cancelled by client";
        var cancellationToken = CancellationToken.None;
        
        var dbOrder = new OrderDb { Id = orderId, Status = OrderStatus.New };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync(dbOrder);
            
        _orderRepositoryMock.Setup(x => x.UpdateAsync(dbOrder, cancellationToken))
            .ReturnsAsync(true);

        var result = await _orderService.SetCancelStatusAsync(orderId, comment, cancellationToken);

        result.Should().BeTrue();
        dbOrder.Status.Should().Be(OrderStatus.Cancelled);
        dbOrder.CancelledComment.Should().Be(comment);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenOrderExists()
    {
        var orderId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        
        var dbOrder = new OrderDb { Id = orderId, Deleted = false };

        _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, cancellationToken))
            .ReturnsAsync(dbOrder);
            
        _orderRepositoryMock.Setup(x => x.UpdateAsync(dbOrder, cancellationToken))
            .ReturnsAsync(true);

        var result = await _orderService.DeleteAsync(orderId, cancellationToken);

        result.Should().BeTrue();
        dbOrder.Deleted.Should().BeTrue();
    }
}