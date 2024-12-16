using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductSizePredictRequest
    {
    
            public float Height { get; set; }
            public float Weight { get; set; }
            public string Brand { get; set; }
            public string Type { get; set; }
            public int Gender { get; set; }
        
    }
}
