using AutoMapper;
using DBStore.Application.DTOs.Auth;
using DBStore.Application.DTOs.Products;
using DBStore.Application.DTOs.Cart;
using DBStore.Application.DTOs.Favorites;
using DBStore.Application.DTOs.Shipping;
using DBStore.Application.DTOs.Orders;
using DBStore.Application.DTOs.Audit;
using DBStore.Domain.Entities;

namespace DBStore.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Auth
            CreateMap<User, UserDto>();
            CreateMap<RegisterDto, User>();
            // el Password lo manejás en el service para hashear
            CreateMap<LoginDto, User>()   // si necesitás mapear algo, opcional
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<User, AuthResponseDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Token, opt => opt.Ignore());

            // Products
            CreateMap<Product, ProductDto>();
            CreateMap<ProductCreateUpdateDto, Product>();

            // Cart
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>();
            CreateMap<CartItemCreateDto, CartItem>();

            // Favorites
            CreateMap<Favorite, FavoriteDto>();

            // Shipping
            CreateMap<ShippingAddress, ShippingAddressDto>();
            CreateMap<ShippingAddressCreateUpdateDto, ShippingAddress>();

            // Orders
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderCreateDto, Order>()
                .ForMember(dest => dest.BillingAddressId, opt => opt.MapFrom(src => src.BillingAddressId))
                // el resto (Items, totales, fechas) lo arma el service
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.Total, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentStatus, opt => opt.Ignore())
                .ForMember(dest => dest.ShippingStatus, opt => opt.Ignore());

            // Audit
            CreateMap<AuditLog, AuditLogDto>();
        }
    }
}
