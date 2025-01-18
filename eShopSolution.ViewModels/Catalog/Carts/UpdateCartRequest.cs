using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Carts
{
    public class UpdateCartRequest
    {
        public Guid UserId { get; set; }
        public List<UpdateCartItem> Items { get; set; }
    }
}
