using System;
using System.Threading.Tasks;
using DBStore.Application.Interfaces;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;

namespace DBStore.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly ICartItemRepository _itemRepo;

        public CartService(ICartRepository cartRepo, ICartItemRepository itemRepo)
        {
            _cartRepo = cartRepo;
            _itemRepo = itemRepo;
        }

        public async Task<Cart> GetOrCreateActiveCartAsync(Guid userId)
        {
            var cart = await _cartRepo.GetActiveByUserAsync(userId);
            if (cart != null) return cart;

            cart = new Cart { UserId = userId, IsActive = true };
            await _cartRepo.AddAsync(cart);
            return cart;
        }

        public async Task AddItemAsync(Guid cartId, Guid productId, int quantity)
        {
            var item = new CartItem { CartId = cartId, ProductId = productId, Quantity = quantity };
            await _itemRepo.AddAsync(item);
        }

        public Task RemoveItemAsync(Guid itemId) =>
            _itemRepo.DeleteAsync(itemId);

        public async Task CheckoutAsync(Guid cartId)
        {
            var cart = await _cartRepo.GetByIdAsync(cartId);
            if (cart == null) throw new InvalidOperationException("Carrito no existe");
            // podés orquestar stock, crear orden, etc.; por ahora solo cerramos el carrito:
            cart.IsActive = false;
            await _cartRepo.UpdateAsync(cart);
        }
    }
}
