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

    
    /// <summary>
    /// Метод создания заказа
    /// </summary>
    /// <param name="orderDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
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

    /// <summary>
    /// Метод получения заказа по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Метод получения списка заказов
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Метод для поиска по заказам
    /// </summary>
    /// <param name="query">Строка запроса для поиска</param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Метод обновления данных заказа
    /// </summary>
    /// <param name="orderDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
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
    
    /// <summary>
    /// Метод отправки заказа курьеру
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="courierId">Идентификатор курьера</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id}/assign/{courierId}")]
    public async Task<ActionResult> AssignToCourier(
        [FromRoute] Guid id, 
        [FromRoute] Guid courierId, 
        CancellationToken cancellationToken)
    {
        try
        {
            var isAssigned  = await _orderService.AssignToCourierAsync(courierId,
                id, 
                cancellationToken);
            if (!isAssigned ) return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assign courier {CourierId} to order {OrderId}", courierId, id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Метод установки статуса заказа "В процессе"
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id}/inprocess")]
    public async Task<ActionResult> SetInProcessStatus(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderService.SetInProcessStatusAsync(id, cancellationToken);
            
            if(!result) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set in process status", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    /// <summary>
    /// Метод установки статуса заказа "Завершено"
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id}/done")]
    public async Task<ActionResult> SetDoneStatus(
        [FromRoute] Guid id, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderService.SetDoneStatusAsync(id, cancellationToken);
        
            if(!result) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set in done status", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    /// <summary>
    /// Метод установки статуса заказа "Отменен"
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPatch("{id}/cancel")]
    public async Task<ActionResult> SetCancelStatus(
        [FromRoute] Guid id, 
        [FromQuery] string comment, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderService.SetCancelStatusAsync(id, comment, cancellationToken);
        
            if(!result) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set in cancel status", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Метод для удаления заказа
    /// </summary>
    /// <param name="id">Идентификатор заказа</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _orderService.DeleteAsync(id, cancellationToken);
        
            if(!result) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete order", id);
            return StatusCode(500, "Internal server error");
        }
    }
}