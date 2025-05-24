using AutoMapper;
using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Models;
using CargoDelivery.Domain.Services;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Enums;
using CargoDelivery.Storage.Interfaces;
using Moq;
using Xunit;

namespace CargoDelivery.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        private readonly Mock<ICourierRepository> _courierRepositoryMock = new();
        private readonly OrderService _orderService;
        private readonly CancellationToken _cancellationToken = CancellationToken.None;

        public OrderServiceTests()
        {
            _orderService = new OrderService(
                _mapperMock.Object,
                _orderRepositoryMock.Object,
                _courierRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedOrders()
        {
            var orderDbs = new List<OrderDb>
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };

            var expectedOrders = new List<Order>
            {
                new() { Id = orderDbs[0].Id },
                new() { Id = orderDbs[1].Id }
            };

            _orderRepositoryMock.Setup(x => x.GetAllAsync(_cancellationToken))
                .ReturnsAsync(orderDbs);

            _mapperMock.Setup(x => x.Map<List<Order>>(orderDbs))
                .Returns(expectedOrders);

            var result = await _orderService.GetAllAsync(_cancellationToken);

            Assert.Equal(expectedOrders, result);
            _orderRepositoryMock.Verify(x => x.GetAllAsync(_cancellationToken), Times.Once);
        }

        [Fact]
        public async Task SearchAsync_ShouldReturnMappedOrders()
        {
            const string query = "test";
            var orderDbs = new List<OrderDb> { new() { Id = Guid.NewGuid() } };
            var expectedOrders = new List<Order> { new() { Id = orderDbs[0].Id } };

            _orderRepositoryMock.Setup(x => x.SearchAsync(query, _cancellationToken))
                .ReturnsAsync(orderDbs);

            _mapperMock.Setup(x => x.Map<List<Order>>(orderDbs))
                .Returns(expectedOrders);

            var result = await _orderService.SearchAsync(query, _cancellationToken);

            Assert.Equal(expectedOrders, result);
            _orderRepositoryMock.Verify(x => x.SearchAsync(query, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedOrder()
        {
            var orderId = Guid.NewGuid();
            var orderDb = new OrderDb { Id = orderId };
            var expectedOrder = new Order { Id = orderId };

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, _cancellationToken))
                .ReturnsAsync(orderDb);

            _mapperMock.Setup(x => x.Map<Order>(orderDb))
                .Returns(expectedOrder);

            var result = await _orderService.GetByIdAsync(orderId, _cancellationToken);

            Assert.Equal(expectedOrder, result);
            _orderRepositoryMock.Verify(x => x.GetByIdAsync(orderId, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnMappedOrder()
        {
            var order = new Order { Id = Guid.NewGuid() };
            var orderDb = new OrderDb { Id = order.Id };
            var createdOrderDb = new OrderDb { Id = order.Id };

            _mapperMock.Setup(x => x.Map<OrderDb>(order))
                .Returns(orderDb);

            _orderRepositoryMock.Setup(x => x.AddAsync(orderDb, _cancellationToken))
                .ReturnsAsync(createdOrderDb);

            _mapperMock.Setup(x => x.Map<Order>(createdOrderDb))
                .Returns(order);

            var result = await _orderService.AddAsync(order, _cancellationToken);

            Assert.Equal(order, result);
            _orderRepositoryMock.Verify(x => x.AddAsync(orderDb, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenOrderStatusIsNotNew_ShouldReturnFalse()
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, Status = OrderStatus.InProcess };
            var orderDb = new OrderDb { Id = orderId, Status = OrderStatus.InProcess };

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, _cancellationToken))
                .ReturnsAsync(orderDb);

            var result = await _orderService.UpdateAsync(order, _cancellationToken);

            Assert.False(result);
            _orderRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<OrderDb>(), _cancellationToken), Times.Never);
        }

        [Fact]
        public async Task AssignToCourierAsync_WhenCourierNotAssigned_ShouldReturnTrue()
        {
            var orderId = Guid.NewGuid();
            var courierId = Guid.NewGuid();
            var orderDb = new OrderDb { Id = orderId, Courier = null };
            var courierDb = new CourierDb { Id = courierId };

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, _cancellationToken))
                .ReturnsAsync(orderDb);

            _courierRepositoryMock.Setup(x => x.GetByIdAsync(courierId, _cancellationToken))
                .ReturnsAsync(courierDb);

            _orderRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<OrderDb>(), _cancellationToken))
                .ReturnsAsync(true);

            var result = await _orderService.AssignToCourierAsync(courierId, orderId, _cancellationToken);

            Assert.True(result);
            Assert.Equal(courierDb, orderDb.Courier);
            _orderRepositoryMock.Verify(x => x.UpdateAsync(orderDb, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task AssignToCourierAsync_WhenCourierAlreadyAssigned_ShouldReturnFalse()
        {
            var orderId = Guid.NewGuid();
            var courierId = Guid.NewGuid();
            var existingCourierId = Guid.NewGuid();
            var orderDb = new OrderDb { Id = orderId, Courier = new CourierDb { Id = existingCourierId } };

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, _cancellationToken))
                .ReturnsAsync(orderDb);

            var result = await _orderService.AssignToCourierAsync(courierId, orderId, _cancellationToken);

            Assert.False(result);
            _orderRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<OrderDb>(), _cancellationToken), Times.Never);
        }

        [Fact]
        public async Task SetInProcessStatusAsync_ShouldUpdateStatusAndReturnTrue()
        {
            var orderId = Guid.NewGuid();
            var orderDb = new OrderDb { Id = orderId, Status = OrderStatus.New };

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, _cancellationToken))
                .ReturnsAsync(orderDb);

            _orderRepositoryMock.Setup(x => x.UpdateAsync(orderDb, _cancellationToken))
                .ReturnsAsync(true);

            var result = await _orderService.SetInProcessStatusAsync(orderId, _cancellationToken);

            Assert.True(result);
            Assert.Equal(OrderStatus.InProcess, (OrderStatus)orderDb.Status.Id);
            _orderRepositoryMock.Verify(x => x.UpdateAsync(orderDb, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task SetDoneStatusAsync_ShouldUpdateStatusAndReturnTrue()
        {
            var orderId = Guid.NewGuid();
            var orderDb = new OrderDb { Id = orderId, Status = OrderStatus.InProcess };

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, _cancellationToken))
                .ReturnsAsync(orderDb);

            _orderRepositoryMock.Setup(x => x.UpdateAsync(orderDb, _cancellationToken))
                .ReturnsAsync(true);

            var result = await _orderService.SetDoneStatusAsync(orderId, _cancellationToken);

            Assert.True(result);
            Assert.Equal(OrderStatus.Done, (OrderStatus)orderDb.Status.Id);
            _orderRepositoryMock.Verify(x => x.UpdateAsync(orderDb, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task SetCancelStatusAsync_ShouldUpdateStatusAndCommentAndReturnTrue()
        {
            var orderId = Guid.NewGuid();
            const string comment = "Cancelled by client";
            var orderDb = new OrderDb { Id = orderId, Status = OrderStatus.New };

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, _cancellationToken))
                .ReturnsAsync(orderDb);

            _orderRepositoryMock.Setup(x => x.UpdateAsync(orderDb, _cancellationToken))
                .ReturnsAsync(true);

            var result = await _orderService.SetCancelStatusAsync(orderId, comment, _cancellationToken);

            Assert.True(result);
            Assert.Equal(OrderStatus.Cancelled, (OrderStatus)orderDb.Status);
            Assert.Equal(comment, orderDb.CancelledComment);
            _orderRepositoryMock.Verify(x => x.UpdateAsync(orderDb, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSetDeletedFlagAndReturnTrue()
        {
            var orderId = Guid.NewGuid();
            var orderDb = new OrderDb { Id = orderId, Deleted = false };

            _orderRepositoryMock.Setup(x => x.GetByIdAsync(orderId, _cancellationToken))
                .ReturnsAsync(orderDb);

            _orderRepositoryMock.Setup(x => x.UpdateAsync(orderDb, _cancellationToken))
                .ReturnsAsync(true);

            var result = await _orderService.DeleteAsync(orderId, _cancellationToken);

            Assert.True(result);
            Assert.True(orderDb.Deleted);
            _orderRepositoryMock.Verify(x => x.UpdateAsync(orderDb, _cancellationToken), Times.Once);
        }
    }
}