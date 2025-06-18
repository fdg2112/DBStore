using DBStore.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStore.Application.DTOs.Favorites
{
    public class FavoriteDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto? Product { get; set; }
    }
}
