using AutoMapper;
using DBStore.Application.DTOs.Favorites;
using DBStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DBStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly IFavoriteService _service;
    private readonly IMapper _mapper;

    public FavoritesController(IFavoriteService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<FavoriteDto>>> Get(Guid userId)
    {
        var favorites = await _service.ListByUserAsync(userId);
        return Ok(_mapper.Map<IEnumerable<FavoriteDto>>(favorites));
    }

    [HttpPost("{userId}/{productId}")]
    public async Task<IActionResult> Toggle(Guid userId, Guid productId)
    {
        await _service.ToggleFavoriteAsync(userId, productId);
        return NoContent();
    }
}
