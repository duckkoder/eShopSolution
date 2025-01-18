using eShopSolution.ViewModels.Catalog.ProductSizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Carts
{
    public class UpdateCartItem
    {
        public int ProductId { get; set; }

        public decimal Price { get; set; }  
        public ProductSizeViewModel Size { get; set; }
    }
}
