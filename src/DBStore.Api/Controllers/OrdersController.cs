using AutoMapper;
using DBStore.Application.DTOs.Orders;
using DBStore.Application.Interfaces;
using DBStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DBStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost("user/{userId}")]
        public async Task<ActionResult<OrderDto>> Create(Guid userId, OrderCreateDto dto)
        {
            var order = await _service.CreateOrderAsync(userId, dto.BillingAddressId);
            var result = _mapper.Map<OrderDto>(order);
            return CreatedAtAction(nameof(GetById), new { id = result.Id, userId }, result);
        }

        [HttpGet("{id}/user/{userId}")]
        public async Task<ActionResult<OrderDto>> GetById(Guid id, Guid userId)
        {
            var order = await _service.GetByIdAsync(id, userId);
            if (order == null) return NotFound();
            return Ok(_mapper.Map<OrderDto>(order));
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetForUser(Guid userId)
        {
            var orders = await _service.ListByUserAsync(userId);
            var result = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            var orders = await _service.ListAllAsync();
            var result = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(result);
        }
    }
}
