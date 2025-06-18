using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStore.Application.DTOs.Cart
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}
