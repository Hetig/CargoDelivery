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
    public async Task<ActionResult<OrderResponseDto>> Create(OrderCreateDto orderDto)
    {
        var order = _mapper.Map<OrderCreateDto, Order>(orderDto);
        var newOrder = await _orderService.AddAsync(order);

        return Created(nameof(GetOrderById), _mapper.Map<Order, OrderResponseDto>(newOrder));
    }

    [HttpGet]
    public async Task<ActionResult<OrderResponseDto>> GetOrderById(Guid id)
    {
        var order = await _orderService.GetByIdAsync(id);

        return Ok(_mapper.Map<Order, OrderResponseDto>(order));
    }
}