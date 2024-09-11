using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModels.Catalog.Products
{
	public class ProductCreateRequest
	{
		[Required(ErrorMessage = "Price is required.")]
		[Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
		public decimal Price { set; get; }

		[Required(ErrorMessage = "Original Price is required.")]
		[Range(0, double.MaxValue, ErrorMessage = "Original Price must be a positive value.")]
		public decimal OriginalPrice { set; get; }

		[Required(ErrorMessage = "Stock is required.")]
		[Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive integer.")]
		public int Stock { set; get; }

		[Required(ErrorMessage = "Product Name is required.")]
		[MaxLength(200, ErrorMessage = "Product Name cannot exceed 200 characters.")]
		public string Name { set; get; }

		[MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
		public string Description { set; get; }

		public string Details { set; get; }

		[MaxLength(500, ErrorMessage = "SEO Description cannot exceed 500 characters.")]
		public string SeoDescription { set; get; }

		[MaxLength(100, ErrorMessage = "SEO Title cannot exceed 100 characters.")]
		public string SeoTitle { set; get; }

		[Required(ErrorMessage = "SEO Alias is required.")]
		public string SeoAlias { get; set; }

		[Required(ErrorMessage = "Language ID is required.")]
		public string LanguageId { set; get; }

		[Required(ErrorMessage = "Thumbnail Image is required.")]
		public IFormFile ThumbnailImage { get; set; }
	}
}
