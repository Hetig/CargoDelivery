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
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IMapper mapper, IOrderService orderService, ILogger<OrdersController> logger)
    {
        _mapper = mapper;
        _orderService = orderService;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult<OrderResponseDto>> Create([FromBody] OrderCreateDto orderDto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
        
            var order = _mapper.Map<OrderCreateDto, Order>(orderDto);
            var newOrder = await _orderService.AddAsync(order, cancellationToken);
        
            if(newOrder == null) return BadRequest();
        
            return Created(nameof(GetById), _mapper.Map<Order, OrderResponseDto>(newOrder));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create order", orderDto);
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderService.GetByIdAsync(id, cancellationToken);
            if (order is null) return NotFound($"Order with id {id} not found");
        
            return Ok(_mapper.Map<Order, OrderResponseDto>(order));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get order by id", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<PaginatedResponseDto<OrderResponseDto>>> GetAll(
        [FromQuery] PaginationRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.PageNumber < 1 || request.PageSize < 1 || request.PageSize > 100)
                return BadRequest("Invalid pagination parameters");

            var result = await _orderService.GetAllPaginatedAsync(
                _mapper.Map<PaginationRequest>(request), cancellationToken);

            return Ok(new PaginatedResponseDto<OrderResponseDto>
            {
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                Data = _mapper.Map<List<OrderResponseDto>>(result.Data)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get paginated orders", request);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<PaginatedResponseDto<OrderResponseDto>>> Search(
        [FromQuery] string query, 
        [FromQuery] PaginationRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.PageNumber < 1 || request.PageSize < 1 || request.PageSize > 100)
                return BadRequest("Invalid pagination parameters");

            var result = await _orderService.SearchPaginatedAsync(
                query,
                _mapper.Map<PaginationRequest>(request),
                cancellationToken);
            
            return Ok(new PaginatedResponseDto<OrderResponseDto>
            {
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                Data = _mapper.Map<List<OrderResponseDto>>(result.Data)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search orders", query);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPatch("update")]
    public async Task<IActionResult> Update(
        [FromBody] OrderUpdateDto orderDto, 
        CancellationToken cancellationToken)
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
            _logger.LogError(ex, "Failed to update order", orderDto);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("{orderId}/assign/{courierId}")]
    public async Task<ActionResult> AssignToCourier(
        [FromRoute] Guid orderId, 
        [FromRoute] Guid courierId, 
        CancellationToken cancellationToken)
    {
        try
        {
            var isAssigned  = await _orderService.AssignToCourierAsync(courierId,
                orderId, 
                cancellationToken);
            if (!isAssigned ) return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assign courier {CourierId} to order {OrderId}", courierId, orderId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("inprocess/{orderId}")]
    public async Task<ActionResult> SetInProcessStatus(
        [FromRoute] Guid orderId, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderService.SetInProcessStatusAsync(orderId, cancellationToken);
            
            if(!result) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set in process status", orderId);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("done/{orderId}")]
    public async Task<ActionResult> SetDoneStatus(
        [FromRoute] Guid orderId, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderService.SetDoneStatusAsync(orderId, cancellationToken);
        
            if(!result) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set in done status", orderId);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("cancel/{orderId}")]
    public async Task<ActionResult> SetCancelStatus(
        [FromRoute] Guid orderId, 
        [FromQuery] string comment, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderService.SetCancelStatusAsync(orderId, comment, cancellationToken);
        
            if(!result) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set in cancel status", orderId);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("delete/{orderId}")]
    public async Task<ActionResult> Delete([FromRoute] Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderService.DeleteAsync(orderId, cancellationToken);
        
            if(!result) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete order", orderId);
            return StatusCode(500, "Internal server error");
        }
    }
}