using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStore.Application.DTOs.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string PaymentStatus { get; set; } = null!;
        public string ShippingStatus { get; set; } = null!;
        public decimal Total { get; set; }
        public IEnumerable<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
