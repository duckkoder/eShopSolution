using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Carts
{
    public class CartViewModel
    {
        public Guid UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal Total { get; set; }
    }
}
