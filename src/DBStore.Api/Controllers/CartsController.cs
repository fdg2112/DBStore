using AutoMapper;
using DBStore.Application.DTOs.Cart;
using DBStore.Application.Interfaces;
using DBStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DBStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _service;
        private readonly IMapper _mapper;

        public CartsController(ICartService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<CartDto>> GetForUser(Guid userId)
        {
            var cart = await _service.GetOrCreateActiveCartAsync(userId);
            var result = _mapper.Map<CartDto>(cart);
            return Ok(result);
        }

        [HttpPost("{cartId}/items")]
        public async Task<IActionResult> AddItem(Guid cartId, CartItemCreateDto dto)
        {
            await _service.AddItemAsync(cartId, dto.ProductId, dto.Quantity);
            return NoContent();
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            await _service.RemoveItemAsync(itemId);
            return NoContent();
        }

        [HttpPost("{cartId}/checkout")]
        public async Task<IActionResult> Checkout(Guid cartId)
        {
            await _service.CheckoutAsync(cartId);
            return NoContent();
        }
    }
}
