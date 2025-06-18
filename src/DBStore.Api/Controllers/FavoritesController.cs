using AutoMapper;
using DBStore.Application.DTOs.Favorites;
using DBStore.Application.Interfaces;
using DBStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DBStore.Api.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _service;
        private readonly IMapper _mapper;

        public FavoritesController(IFavoriteService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavoriteDto>>> Get(Guid userId)
        {
            var favs = await _service.ListByUserAsync(userId);
            var result = _mapper.Map<IEnumerable<FavoriteDto>>(favs);
            return Ok(result);
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> Toggle(Guid userId, Guid productId)
        {
            await _service.ToggleFavoriteAsync(userId, productId);
            return NoContent();
        }
    }
}
