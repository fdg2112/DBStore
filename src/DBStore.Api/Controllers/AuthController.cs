using System;
using System.IdentityModel.Tokens.Jwt;
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
using Microsoft.IdentityModel.Tokens;

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


        public AuthController(
            IUserRepository userRepo,
            IRoleRepository roleRepo,
            IPasswordHasher<User> hasher,
            IMapper mapper,
            IConfiguration config)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _hasher = hasher;
            _mapper = mapper;
            _config = config;
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

            // 1) Creo el usuario en la BD
            await _userRepo.AddAsync(user);

            // 2) Asigno rol 'client'
            var clientRole = await _roleRepo.GetByNameAsync("client");
            if (clientRole == null)
                return StatusCode(500, "El rol 'client' no está en la base");

            var db = HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
            db.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = clientRole.Id
            });
            await db.SaveChangesAsync();

            // 3) Genero el JWT con rol 'client'
            var token = GenerateJwt(user, "client");
            var userDto = _mapper.Map<UserDto>(user);

            return Created("", new AuthResponseDto { User = userDto, Token = token });
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

            // Por ahora todos son 'user'; si hacés roles reales, sacalos de la BD.
            var token = GenerateJwt(user, "user");
            var userDto = _mapper.Map<UserDto>(user);

            return Ok(new AuthResponseDto
            {
                User = userDto,
                Token = token
            });
        }

        private string GenerateJwt(User user, string role)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
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
