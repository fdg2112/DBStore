using AutoMapper;
using DBStore.Application.DTOs.Audit;
using DBStore.Application.Interfaces;
using DBStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DBStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _service;
        private readonly IMapper _mapper;

        public AuditLogsController(IAuditLogService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAll()
        {
            var logs = await _service.ListAllAsync();
            var result = _mapper.Map<IEnumerable<AuditLogDto>>(logs);
            return Ok(result);
        }
    }
}
