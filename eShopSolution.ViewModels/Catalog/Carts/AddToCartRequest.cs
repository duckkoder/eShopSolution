using eShopSolution.ViewModels.Catalog.ProductSizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Carts
{
    public class AddToCartRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Price { get; set; }
        public ProductSizeViewModel ProductSize { get; set; }
    }
}
