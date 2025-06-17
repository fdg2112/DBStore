using System;
using System.Collections.Generic;

namespace DBStore.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? BillingAddressId { get; set; }
        public string PaymentStatus { get; set; } = null!;
        public string ShippingStatus { get; set; } = null!;
        public string? TransactionId { get; set; }
        public decimal Total { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
