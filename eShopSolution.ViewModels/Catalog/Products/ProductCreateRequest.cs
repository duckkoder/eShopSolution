﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModels.Catalog.Products
{
	public class ProductCreateRequest
	{
		public decimal Price { set; get; }
		public decimal OriginalPrice { set; get; }
		public int BrandId { set; get; }
		public string Name { set; get; }
		public string Description { set; get; }
		public string Details { set; get; }
		public string SeoDescription { set; get; }

		public string SeoTitle { set; get; }
		public string SeoAlias { get; set; }
		public string ?LanguageId { set; get; }
		public IFormFile ?ThumbnailImage { get; set; }
	}
}
