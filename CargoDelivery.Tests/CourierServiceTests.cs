using AutoMapper;
using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Models;
using CargoDelivery.Domain.Services;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CargoDelivery.Tests;

public class CourierServiceTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ICourierRepository> _courierRepositoryMock = new();
    private readonly CourierService _courierService;

    public CourierServiceTests()
    {
        _courierService = new CourierService(
            _mapperMock.Object,
            _courierRepositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCourier_WhenCourierExists()
    {
        var courierId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        
        var dbCourier = new CourierDb { Id = courierId, Name = "Test Courier" };
        var expectedCourier = new Courier { Id = courierId, Name = "Test Courier" };

        _courierRepositoryMock.Setup(x => x.GetByIdAsync(courierId, cancellationToken))
            .ReturnsAsync(dbCourier);
        
        _mapperMock.Setup(x => x.Map<Courier>(dbCourier))
            .Returns(expectedCourier);

        var result = await _courierService.GetByIdAsync(courierId, cancellationToken);

        result.Should().NotBeNull();
        result.Id.Should().Be(courierId);
        result.Name.Should().Be("Test Courier");
        
        _courierRepositoryMock.Verify(x => x.GetByIdAsync(courierId, cancellationToken), Times.Once);
        _mapperMock.Verify(x => x.Map<Courier>(dbCourier), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenCourierNotExists()
    {
        var courierId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _courierRepositoryMock.Setup(x => x.GetByIdAsync(courierId, cancellationToken))
            .ReturnsAsync((CourierDb?)null);

        var result = await _courierService.GetByIdAsync(courierId, cancellationToken);

        result.Should().BeNull();
        
        _courierRepositoryMock.Verify(x => x.GetByIdAsync(courierId, cancellationToken), Times.Once);
        _mapperMock.Verify(x => x.Map<Courier>(It.IsAny<CourierDb>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllCouriers()
    {
        var cancellationToken = CancellationToken.None;
        
        var dbCouriers = new List<CourierDb>
        {
            new() { Id = Guid.NewGuid(), Name = "Courier 1" },
            new() { Id = Guid.NewGuid(), Name = "Courier 2" }
        };
        
        var expectedCouriers = new List<Courier>
        {
            new() { Id = dbCouriers[0].Id, Name = "Courier 1" },
            new() { Id = dbCouriers[1].Id, Name = "Courier 2" }
        };

        _courierRepositoryMock.Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(dbCouriers);
        
        _mapperMock.Setup(x => x.Map<List<Courier>>(dbCouriers))
            .Returns(expectedCouriers);

        var result = await _courierService.GetAllAsync(cancellationToken);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Courier 1");
        result[1].Name.Should().Be("Courier 2");
        
        _courierRepositoryMock.Verify(x => x.GetAllAsync(cancellationToken), Times.Once);
        _mapperMock.Verify(x => x.Map<List<Courier>>(dbCouriers), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoCouriersExist()
    {
        var cancellationToken = CancellationToken.None;

        _courierRepositoryMock.Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(new List<CourierDb>());
        
        _mapperMock.Setup(x => x.Map<List<Courier>>(It.IsAny<List<CourierDb>>()))
            .Returns(new List<Courier>());

        var result = await _courierService.GetAllAsync(cancellationToken);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddAsync_ReturnsCreatedCourier()
    {
        var courier = new Courier { Name = "New Courier"};
        var cancellationToken = CancellationToken.None;
        
        var dbCourier = new CourierDb { Id = Guid.NewGuid(), Name = courier.Name};
        var expectedCourier = new Courier { Id = dbCourier.Id, Name = courier.Name };

        _mapperMock.Setup(x => x.Map<CourierDb>(courier))
            .Returns(dbCourier);
            
        _courierRepositoryMock.Setup(x => x.AddAsync(dbCourier, cancellationToken))
            .ReturnsAsync(dbCourier);
            
        _mapperMock.Setup(x => x.Map<Courier>(dbCourier))
            .Returns(expectedCourier);

        var result = await _courierService.AddAsync(courier, cancellationToken);

        result.Should().NotBeNull();
        result.Id.Should().Be(dbCourier.Id);
        result.Name.Should().Be(courier.Name);
        
        _mapperMock.Verify(x => x.Map<CourierDb>(courier), Times.Once);
        _courierRepositoryMock.Verify(x => x.AddAsync(dbCourier, cancellationToken), Times.Once);
        _mapperMock.Verify(x => x.Map<Courier>(dbCourier), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ProperlyMapsAndSavesCourier()
    {
        var courier = new Courier 
        { 
            Name = "Test Courier", 
        };
        
        var cancellationToken = CancellationToken.None;
        
        var dbCourier = new CourierDb 
        { 
            Id = Guid.NewGuid(), 
            Name = courier.Name,
        };

        _mapperMock.Setup(x => x.Map<CourierDb>(courier))
            .Returns(dbCourier);
            
        _courierRepositoryMock.Setup(x => x.AddAsync(dbCourier, cancellationToken))
            .ReturnsAsync(dbCourier);
            
        _mapperMock.Setup(x => x.Map<Courier>(dbCourier))
            .Returns(courier);

        var result = await _courierService.AddAsync(courier, cancellationToken);

        result.Should().BeEquivalentTo(courier);
        
        _mapperMock.Verify(x => x.Map<CourierDb>(It.Is<Courier>(c => 
            c.Name == courier.Name)), 
        Times.Once);
    }
}