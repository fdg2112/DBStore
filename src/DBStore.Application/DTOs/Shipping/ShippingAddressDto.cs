using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStore.Application.DTOs.Shipping
{
    public class ShippingAddressDto
    {
        public Guid Id { get; set; }
        public string AddressLine { get; set; } = null!;
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public string ShippingMethod { get; set; } = null!;
        public decimal Cost { get; set; }
        public string? Carrier { get; set; }
    }
}
