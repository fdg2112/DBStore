using AutoMapper;
using DBStore.Application.DTOs.Shipping;
using DBStore.Application.Interfaces;
using DBStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DBStore.Api.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    public class ShippingAddressesController : ControllerBase
    {
        private readonly IShippingAddressService _service;
        private readonly IMapper _mapper;

        public ShippingAddressesController(IShippingAddressService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingAddressDto>>> List(Guid userId)
        {
            var addresses = await _service.ListByUserAsync(userId);
            var result = _mapper.Map<IEnumerable<ShippingAddressDto>>(addresses);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ShippingAddressDto>> Create(Guid userId, ShippingAddressCreateUpdateDto dto)
        {
            var entity = _mapper.Map<ShippingAddress>(dto);
            entity.UserId = userId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity = await _service.AddAsync(entity);
            var result = _mapper.Map<ShippingAddressDto>(entity);
            return CreatedAtAction(nameof(GetById), new { userId, id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShippingAddressDto>> GetById(Guid userId, Guid id)
        {
            var addresses = await _service.ListByUserAsync(userId);
            var entity = addresses.FirstOrDefault(a => a.Id == id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<ShippingAddressDto>(entity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid userId, Guid id, ShippingAddressCreateUpdateDto dto)
        {
            var entity = _mapper.Map<ShippingAddress>(dto);
            entity.Id = id;
            entity.UserId = userId;
            entity.UpdatedAt = DateTime.UtcNow;
            await _service.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid userId, Guid id)
        {
            await _service.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}
