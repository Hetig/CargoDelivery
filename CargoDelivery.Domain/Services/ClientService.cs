using AutoMapper;
using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Models;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Interfaces;

namespace CargoDelivery.Domain.Services;

public class ClientService : IClientService
{
    private readonly IMapper _mapper;
    private readonly IClientRepository _clientRepository;

    public ClientService(IMapper _mapper, IClientRepository clientRepository)
    {
        this._mapper = _mapper;
        _clientRepository = clientRepository;
    }
    
    public async Task<Client> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var clientDb = await _clientRepository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<Client>(clientDb);
    }

    public async Task<List<Client>> GetAllAsync(CancellationToken cancellationToken)
    {
        var clientsDb = await _clientRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<Client>>(clientsDb);
    }

    public async Task<Client> AddAsync(Client client, CancellationToken cancellationToken)
    {
        var createdClient = await _clientRepository.AddAsync(_mapper.Map<ClientDb>(client), cancellationToken);
        return _mapper.Map<Client>(createdClient);
    }
}