using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.ProductImages
{
	public class ProductImageCreateRequest
	{
		public int id { get; set; }
		public string Caption { get; set; } = "";
		public bool IsDefault { get; set; } = false;
		public int SortOrder { get; set; } = 0;

		public IFormFile ImageFile {  get; set; } 
	}
}
