﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Products
{
	public class ProductViewModel
	{
		public int Id { get; set; }
		public decimal Price { get; set; }
		public int BrandId { get; set; }
		public string BrandName { get; set; }
		public decimal OriginalPrice { get; set; }
		public int Stock { get; set; }
		public int ViewCount { get; set; }
		public DateTime DateCreated { get; set; }
		public string Name { set; get; }
		public string Description { set; get; }
		public string Details { set; get; }
		public string SeoDescription { set; get; }
		public string SeoTitle { set; get; }
		public string SeoAlias { get; set; }
		public string LanguageId { set; get; }
		public string ImageThumbnails { set; get; }
		public  List<string> ?Images { set; get; } = new List<string>();
        public List<string> ?Categories { get; set; } = new List<string>();
    }
}
