using AutoMapper;
using DBStore.Application.DTOs.Products;
using DBStore.Application.Interfaces;
using DBStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DBStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductsController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _service.ListAllAsync();
            var result = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(Guid id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create(ProductCreateUpdateDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            product = await _service.CreateAsync(product);
            var result = _mapper.Map<ProductDto>(product);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ProductCreateUpdateDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            product.Id = id;
            await _service.UpdateAsync(product);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
