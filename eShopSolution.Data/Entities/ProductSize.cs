using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class ProductSize
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int SizeId { get; set; }
        public Size Size { get; set; } 
        public int Quantity { get; set; } 
    }

}
