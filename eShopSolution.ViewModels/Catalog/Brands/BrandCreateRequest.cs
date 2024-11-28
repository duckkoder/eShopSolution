﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Brands
{
    public class BrandCreateRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile? ThumbnailImage { get; set; }
    }
}
