// src/DBStore.Api/Controllers/AuthController.cs
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DBStore.Application.DTOs.Auth;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;
using DBStore.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace DBStore.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _db;

        public AuthController(
            IUserRepository userRepo,
            IRoleRepository roleRepo,
            IPasswordHasher<User> hasher,
            IMapper mapper,
            IConfiguration config,
            ApplicationDbContext db)        // inyectamos el contexto
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _hasher = hasher;
            _mapper = mapper;
            _config = config;
            _db = db;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                return Conflict("Ya existe un usuario con ese email");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EmailVerified = false
            };
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            // 1) Creo el usuario
            await _userRepo.AddAsync(user);

            // 2) Asigno rol 'client'
            var clientRole = await _roleRepo.GetByNameAsync("client");
            if (clientRole == null)
                return StatusCode(500, "El rol 'client' no está en la base");

            _db.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = clientRole.Id
            });
            await _db.SaveChangesAsync();

            // 3) Genero el JWT con rol real
            var roles = new[] { clientRole.Name };
            var token = GenerateJwt(user, roles);
            var userDto = _mapper.Map<UserDto>(user);

            return Created(string.Empty, new AuthResponseDto { User = userDto, Token = token });
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Credenciales inválidas");

            var res = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (res == PasswordVerificationResult.Failed)
                return Unauthorized("Credenciales inválidas");

            // 1) Obtengo los roles reales desde la BD
            var roleNames = await _db.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(_db.Roles,
                      ur => ur.RoleId,
                      r => r.Id,
                      (ur, r) => r.Name)
                .ToListAsync();

            // 2) Genero el JWT con esos roles
            var token = GenerateJwt(user, roleNames);
            var userDto = _mapper.Map<UserDto>(user);

            return Ok(new AuthResponseDto { User = userDto, Token = token });
        }

        // Ahora soporta múltiples roles
        private string GenerateJwt(User user, IEnumerable<string> roles)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString())
            }
            .Concat(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
