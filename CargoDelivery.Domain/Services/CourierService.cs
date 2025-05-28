using AutoMapper;
using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Models;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Interfaces;

namespace CargoDelivery.Domain.Services;

public class CourierService : ICourierService
{
    private readonly IMapper _mapper;
    private readonly ICourierRepository _courierRepository;

    public CourierService(IMapper _mapper, ICourierRepository courierRepository)
    {
        this._mapper = _mapper;
        _courierRepository = courierRepository;
    }
    
    public async Task<Courier> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var courierDb = await _courierRepository.GetByIdAsync(id, cancellationToken);
        return courierDb is not null ? _mapper.Map<Courier>(courierDb) : null;
    }

    public async Task<List<Courier>> GetAllAsync(CancellationToken cancellationToken)
    {
        var couriersDb = await _courierRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<Courier>>(couriersDb);
    }

    public async Task<Courier> AddAsync(Courier client, CancellationToken cancellationToken)
    {
        var createdCourier = await _courierRepository.AddAsync(_mapper.Map<CourierDb>(client), cancellationToken);
        return _mapper.Map<Courier>(createdCourier);
    }
}