﻿using eShopSolution.ViewModels.Catalog.ProductSizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Products
{
    public class ProductDetailViewModel
    {
        public ProductViewModel product { get; set; }

        public ProductSizePredictRequest predictRequest { get; set; }
        public List<ProductSizeViewModel> ProductSizes { get; set; }
    }
}