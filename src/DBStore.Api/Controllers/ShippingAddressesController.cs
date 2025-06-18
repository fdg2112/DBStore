using AutoMapper;
using DBStore.Application.DTOs.Shipping;
using DBStore.Application.Interfaces;
using DBStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DBStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippingAddressesController : ControllerBase
{
    private readonly IShippingAddressService _service;
    private readonly IMapper _mapper;

    public ShippingAddressesController(IShippingAddressService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ShippingAddressDto>>> GetByUser(Guid userId)
    {
        var addresses = await _service.ListByUserAsync(userId);
        return Ok(_mapper.Map<IEnumerable<ShippingAddressDto>>(addresses));
    }

    [HttpPost("user/{userId}")]
    public async Task<ActionResult<ShippingAddressDto>> Create(Guid userId, ShippingAddressCreateUpdateDto dto)
    {
        var address = _mapper.Map<ShippingAddress>(dto);
        address.UserId = userId;
        var created = await _service.AddAsync(address);
        var result = _mapper.Map<ShippingAddressDto>(created);
        return CreatedAtAction(nameof(GetByUser), new { userId }, result);
    }

    [HttpPut("{id}/user/{userId}")]
    public async Task<IActionResult> Update(Guid id, Guid userId, ShippingAddressCreateUpdateDto dto)
    {
        var address = _mapper.Map<ShippingAddress>(dto);
        address.Id = id;
        address.UserId = userId;
        await _service.UpdateAsync(address);
        return NoContent();
    }

    [HttpDelete("{id}/user/{userId}")]
    public async Task<IActionResult> Delete(Guid id, Guid userId)
    {
        await _service.DeleteAsync(id, userId);
        return NoContent();
    }
}
