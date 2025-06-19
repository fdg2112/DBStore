using AutoMapper;
using DBStore.Application.DTOs.Audit;
using DBStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DBStore.Api.Controllers
{
    [ApiController]
    [Route("api/audit-logs")]
    [Authorize(Roles = "admin")]
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
        [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), 200)]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAll()
        {
            var logs = await _service.ListAllAsync();
            var result = _mapper.Map<IEnumerable<AuditLogDto>>(logs);
            return Ok(result);
        }
    }
}
