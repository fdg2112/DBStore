using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBStore.Application.Interfaces;
using DBStore.Domain.Contracts;
using DBStore.Domain.Entities;

namespace DBStore.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICartRepository _cartRepo;
        private readonly IShippingAddressRepository _addrRepo;
        private readonly IProductRepository _prodRepo;

        public OrderService(
            IOrderRepository orderRepo,
            ICartRepository cartRepo,
            IShippingAddressRepository addrRepo,
            IProductRepository prodRepo)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _addrRepo = addrRepo;
            _prodRepo = prodRepo;
        }

        public async Task<Order> CreateOrderAsync(Guid userId, Guid billingAddressId)
        {
            // 1) Traer o validar carrito
            var cart = await _cartRepo.GetActiveByUserAsync(userId)
                      ?? throw new InvalidOperationException("No hay carrito activo");

            // 2) Validar dirección
            var address = await _addrRepo.GetByIdAsync(billingAddressId)
                          ?? throw new InvalidOperationException("Dirección inválida");

            // 3) Crear orden y poblarla con items
            var order = new Order
            {
                UserId = userId,
                BillingAddressId = billingAddressId,
                PaymentStatus = "pending",
                ShippingStatus = "pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            foreach (var ci in cart.Items)
            {
                var product = await _prodRepo.GetByIdAsync(ci.ProductId)
                              ?? throw new InvalidOperationException($"Producto {ci.ProductId} no existe");

                // Decrementar stock si querés
                product.Stock -= ci.Quantity;
                await _prodRepo.UpdateAsync(product);

                var orderItem = new OrderItem
                {
                    OrderId = order.Id,           // EF lo rellenará al guardar
                    ProductId = product.Id,
                    Quantity = ci.Quantity,
                    Price = product.Price
                };
                order.Items.Add(orderItem);
            }

            // 4) Calcular total
            order.Total = order.Items.Sum(oi => oi.Quantity * oi.Price);

            // 5) Guardar orden y “cerrar” carrito
            await _orderRepo.AddAsync(order);
            cart.IsActive = false;
            await _cartRepo.UpdateAsync(cart);

            return order;
        }

        public Task<Order?> GetByIdAsync(Guid id, Guid userId) =>
            _orderRepo.GetByIdAsync(id);

        public Task<IEnumerable<Order>> ListByUserAsync(Guid userId) =>
            _orderRepo.ListByUserAsync(userId);

        public Task<IEnumerable<Order>> ListAllAsync() =>
            _orderRepo.ListAllAsync();
    }
}
