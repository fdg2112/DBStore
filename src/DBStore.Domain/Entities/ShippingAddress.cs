using System;

namespace DBStore.Domain.Entities
{
    public class ShippingAddress
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string AddressLine { get; set; } = null!;
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public string ShippingMethod { get; set; } = null!;
        public decimal Cost { get; set; }
        public string? Carrier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
