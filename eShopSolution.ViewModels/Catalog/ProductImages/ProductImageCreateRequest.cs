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
		public string Caption { get; set; }
		public bool IsDefault { get; set; }
		public int SortOrder { get; set; }

		public IFormFile ImageFile {  get; set; }
	}
}
