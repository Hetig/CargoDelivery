using AutoMapper;
using CargoDelivery.API.Dtos;
using CargoDelivery.Domain.Interfaces;
using CargoDelivery.Domain.Models;
using CargoDelivery.Storage.Entities;
using CargoDelivery.Storage.Enums;
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

    [HttpPost("create")]
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
        if (order is null) return NotFound($"Client with id {id} not found");
        
        return Ok(_mapper.Map<Order, OrderResponseDto>(order));
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAllAsync(cancellationToken);
        return Ok(_mapper.Map<List<Order>, List<OrderResponseDto>>(orders));
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<List<OrderResponseDto>>> Search([FromQuery] string query, CancellationToken cancellationToken)
    {
        return Ok(await _orderService.SearchAsync(query, cancellationToken));
    }
    
    [HttpPatch("update")]
    public async Task<IActionResult> Update([FromBody] OrderUpdateDto orderDto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var order = _mapper.Map<OrderUpdateDto, Order>(orderDto);
            var result = await _orderService.UpdateAsync(order, cancellationToken);
            
            if (!result) return NotFound();
            return Ok(order);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("{orderId}/assign/{courierId}")]
    public async Task<ActionResult> AssignToCourier([FromRoute] Guid orderId, [FromRoute] Guid courierId, CancellationToken cancellationToken)
    {
        var result = await _orderService.AssignToCourierAsync(courierId,
                                                                    orderId, 
                                                                    cancellationToken);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPost("inprocess/{orderId}")]
    public async Task<ActionResult> SetInProcessStatus([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var result = await _orderService.SetInProcessStatusAsync(orderId, cancellationToken);
        
        if(!result) return NotFound();
        return NoContent();
    }
    
    [HttpPost("done/{orderId}")]
    public async Task<ActionResult> SetDoneStatus([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var result = await _orderService.SetDoneStatusAsync(orderId, cancellationToken);
        
        if(!result) return NotFound();
        return NoContent();
    }
    
    [HttpPost("cancel/{orderId}")]
    public async Task<ActionResult> SetCancelStatus([FromRoute] Guid orderId, [FromQuery] string comment, CancellationToken cancellationToken)
    {
        var result = await _orderService.SetCancelStatusAsync(orderId, comment, cancellationToken);
        
        if(!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("delete/{orderId}")]
    public async Task<ActionResult> Delete([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        var result = await _orderService.DeleteAsync(orderId, cancellationToken);
        
        if(!result) return NotFound();
        return NoContent();
    }
}