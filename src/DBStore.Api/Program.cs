using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DBStore.Application.Mapping;
using DBStore.Infrastructure.Data;
using DBStore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;
using DBStore.Domain.Contracts;
using DBStore.Application.Interfaces;
using DBStore.Application.Services;
using DBStore.Application;
using DBStore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1) Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // (opcional) Si querés nombrar tu doc:
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DBStore API", Version = "v1" });

    // Definición del esquema de seguridad "Bearer"
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese **Bearer &lt;su_token&gt;** para autenticarse."
    });

    // Requisito global: todos los endpoints usan ese esquema
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 2) CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
});

// 3) EF Core + Supabase
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseNpgsql(conn)
        .UseSnakeCaseNamingConvention()
);

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// 4) Repositorios
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IRoleRepository, EfRoleRepository>();
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<ICartRepository, EfCartRepository>();
builder.Services.AddScoped<ICartItemRepository, EfCartItemRepository>();
builder.Services.AddScoped<IFavoriteRepository, EfFavoriteRepository>();
builder.Services.AddScoped<IShippingAddressRepository, EfShippingAddressRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, EfOrderItemRepository>();
builder.Services.AddScoped<IAuditLogRepository, EfAuditLogRepository>();

// 5) Servicios de aplicación
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IShippingAddressService, ShippingAddressService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();

// 6) AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// 7) Autenticación JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
var jwtIss = builder.Configuration["Jwt:Issuer"];
var jwtAud = builder.Configuration["Jwt:Audience"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.RequireHttpsMetadata = true;
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = jwtIss,
            ValidateAudience = true,
            ValidAudience = jwtAud,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// 8) Controllers
builder.Services.AddControllers();

var app = builder.Build();

// Pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DBStore API V1");
    c.RoutePrefix = "swagger";  // puedes omitir para dejarlo en la raíz
});

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


// CORS
app.UseCors();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// (Opcional) El viejo weatherforecast si lo querés seguir teniendo:
// app.MapGet("/weatherforecast", () => /* ... */);

// Un endpoint simple para que el load-balancer sepa que la app está viva
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }))
   .ExcludeFromDescription();  // opcional: no aparece en Swagger

app.Run();
