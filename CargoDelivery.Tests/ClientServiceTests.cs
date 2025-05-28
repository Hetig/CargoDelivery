
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

public class ClientServiceTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IClientRepository> _clientRepositoryMock = new();
    private readonly ClientService _clientService;

    public ClientServiceTests()
    {
        _clientService = new ClientService(
            _mapperMock.Object,
            _clientRepositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedClient_WhenClientExists()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
    
        var dbClient = new ClientDb { Id = clientId, Name = "Test Client" };
        var expectedClient = new Client { Id = clientId, Name = "Test Client" };

        _clientRepositoryMock.Setup(x => x.GetByIdAsync(clientId, cancellationToken))
            .ReturnsAsync(dbClient);
    
        _mapperMock.Setup(x => x.Map<Client>(dbClient))
            .Returns(expectedClient);

        // Act
        var result = await _clientService.GetByIdAsync(clientId, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(clientId);
        result.Name.Should().Be("Test Client");
    
        _clientRepositoryMock.Verify(x => x.GetByIdAsync(clientId, cancellationToken), Times.Once);
        _mapperMock.Verify(x => x.Map<Client>(dbClient), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenClientNotExists()
    {
        var clientId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _clientRepositoryMock.Setup(x => x.GetByIdAsync(clientId, cancellationToken))
            .ReturnsAsync((ClientDb?)null);

        var result = await _clientService.GetByIdAsync(clientId, cancellationToken);

        result.Should().BeNull();
    
        _clientRepositoryMock.Verify(x => x.GetByIdAsync(clientId, cancellationToken), Times.Once);
        _mapperMock.Verify(x => x.Map<Client>(It.IsAny<ClientDb>()), Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllClients()
    {
        var cancellationToken = CancellationToken.None;
        
        var dbClients = new List<ClientDb>
        {
            new() { Id = Guid.NewGuid(), Name = "Client 1" },
            new() { Id = Guid.NewGuid(), Name = "Client 2" }
        };
        
        var expectedClients = new List<Client>
        {
            new() { Id = dbClients[0].Id, Name = "Client 1" },
            new() { Id = dbClients[1].Id, Name = "Client 2" }
        };

        _clientRepositoryMock.Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(dbClients);
        
        _mapperMock.Setup(x => x.Map<List<Client>>(dbClients))
            .Returns(expectedClients);

        var result = await _clientService.GetAllAsync(cancellationToken);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Client 1");
        result[1].Name.Should().Be("Client 2");
        
        _clientRepositoryMock.Verify(x => x.GetAllAsync(cancellationToken), Times.Once);
        _mapperMock.Verify(x => x.Map<List<Client>>(dbClients), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoClientsExist()
    {
        var cancellationToken = CancellationToken.None;

        _clientRepositoryMock.Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(new List<ClientDb>());
        
        _mapperMock.Setup(x => x.Map<List<Client>>(It.IsAny<List<ClientDb>>()))
            .Returns(new List<Client>());

        var result = await _clientService.GetAllAsync(cancellationToken);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddAsync_ReturnsCreatedClient()
    {
        var client = new Client { Name = "New Client" };
        var cancellationToken = CancellationToken.None;
        
        var dbClient = new ClientDb { Id = Guid.NewGuid(), Name = client.Name };
        var expectedClient = new Client { Id = dbClient.Id, Name = client.Name };

        _mapperMock.Setup(x => x.Map<ClientDb>(client))
            .Returns(dbClient);
            
        _clientRepositoryMock.Setup(x => x.AddAsync(dbClient, cancellationToken))
            .ReturnsAsync(dbClient);
            
        _mapperMock.Setup(x => x.Map<Client>(dbClient))
            .Returns(expectedClient);

        var result = await _clientService.AddAsync(client, cancellationToken);

        result.Should().NotBeNull();
        result.Id.Should().Be(dbClient.Id);
        result.Name.Should().Be(client.Name);
        
        _mapperMock.Verify(x => x.Map<ClientDb>(client), Times.Once);
        _clientRepositoryMock.Verify(x => x.AddAsync(dbClient, cancellationToken), Times.Once);
        _mapperMock.Verify(x => x.Map<Client>(dbClient), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ProperlyMapsAndSavesClient()
    {
        var client = new Client 
        { 
            Name = "Test Client", 
        };
        
        var cancellationToken = CancellationToken.None;
        
        var dbClient = new ClientDb 
        { 
            Id = Guid.NewGuid(), 
            Name = client.Name,
        };

        _mapperMock.Setup(x => x.Map<ClientDb>(client))
            .Returns(dbClient);
            
        _clientRepositoryMock.Setup(x => x.AddAsync(dbClient, cancellationToken))
            .ReturnsAsync(dbClient);
            
        _mapperMock.Setup(x => x.Map<Client>(dbClient))
            .Returns(client);

        var result = await _clientService.AddAsync(client, cancellationToken);

        result.Should().BeEquivalentTo(client);
        
        _mapperMock.Verify(x => x.Map<ClientDb>(It.Is<Client>(c => 
            c.Name == client.Name)), 
        Times.Once);
    }
}