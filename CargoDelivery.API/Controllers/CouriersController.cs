using AutoMapper;
using CargoDelivery.API.Dtos;
using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CargoDelivery.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CouriersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICourierService _courierService;
    private readonly ILogger<CouriersController> _logger;

    public CouriersController(IMapper mapper, ICourierService courierService, ILogger<CouriersController> logger)
    {
        _mapper = mapper;
        _courierService = courierService;
        _logger = logger;
    }
    
    /// <summary>
    /// Метод для создания курьера
    /// </summary>
    /// <param name="courierDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<CourierResponseDto>> Create([FromBody] CourierCreateDto courierDto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
        
            var courier = _mapper.Map<CourierCreateDto, Courier>(courierDto);
            var newCourier = await _courierService.AddAsync(courier, cancellationToken);
        
            if(newCourier == null) return BadRequest();
        
            return Created(nameof(GetById), _mapper.Map<Courier, CourierResponseDto>(newCourier));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create courier", courierDto);
            throw;
        }
    }
    
    /// <summary>
    /// Метод для получения курьера по идентификатору
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CourierResponseDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var courier = await _courierService.GetByIdAsync(id, cancellationToken);
            if (courier is null) return NotFound($"Courier with id {id} not found");
        
            return Ok(_mapper.Map<Courier, CourierResponseDto>(courier));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get courier by id", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    /// <summary>
    /// Метод получения списка курьеров
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<CourierResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _courierService.GetAllAsync(cancellationToken);
            return Ok(_mapper.Map<List<CourierResponseDto>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all couriers");
            return StatusCode(500, "Internal server error");
        }
    }
}