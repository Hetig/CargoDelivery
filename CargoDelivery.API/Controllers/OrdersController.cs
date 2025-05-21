using AutoMapper;
using CargoDelivery.API.Dtos;
using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CargoDelivery.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;

    public OrdersController(IMapper mapper, IOrderService orderService)
    {
        _mapper = mapper;
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> Create([FromBody] OrderCreateDto orderDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        
        var order = _mapper.Map<OrderCreateDto, Order>(orderDto);
        var newOrder = await _orderService.AddAsync(order, cancellationToken);

        return Created(nameof(GetById), _mapper.Map<Order, OrderResponseDto>(newOrder));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var order = await _orderService.GetByIdAsync(id, cancellationToken);

        return Ok(_mapper.Map<Order, OrderResponseDto>(order));
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<List<Order>, List<OrderResponseDto>>(orders));
    }
}