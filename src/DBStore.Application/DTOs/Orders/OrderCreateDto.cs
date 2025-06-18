using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStore.Application.DTOs.Orders
{
    public class OrderCreateDto
    {
        public Guid BillingAddressId { get; set; }
    }
}
