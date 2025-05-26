using AutoMapper;
using CargoDelivery.API.Dtos;
using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CargoDelivery.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IClientService _clientService;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(IMapper mapper, IClientService clientService, ILogger<ClientsController> logger)
    {
        _mapper = mapper;
        _clientService = clientService;
        _logger = logger;
    }
    
    /// <summary>
    /// Метод создания клиента
    /// </summary>
    /// <param name="clientDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ClientResponseDto>> Create([FromBody] ClientCreateDto clientDto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
        
            var client = _mapper.Map<ClientCreateDto, Client>(clientDto);
            var newClient = await _clientService.AddAsync(client, cancellationToken);
        
            if(newClient == null) return BadRequest();
        
            return Created(nameof(GetById), _mapper.Map<Client, ClientResponseDto>(newClient));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create order", clientDto);
            throw;
        }
    }
    
    /// <summary>
    /// Метод получения клиента по идентификатору
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientResponseDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var client = await _clientService.GetByIdAsync(id, cancellationToken);
            if (client is null) return NotFound($"Client with id {id} not found");
        
            return Ok(_mapper.Map<Client, ClientResponseDto>(client));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get client by id", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    /// <summary>
    /// Метод получения списка клиентов
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<ClientResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _clientService.GetAllAsync(cancellationToken);
            return Ok(_mapper.Map<List<ClientResponseDto>>(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get all clients");
            return StatusCode(500, "Internal server error");
        }
    }
}