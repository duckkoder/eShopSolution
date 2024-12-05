using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModels.Catalog.Products
{
	public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
	{
		public ProductCreateRequestValidator() {

			RuleFor(x => x.Price)
				.NotEmpty().WithMessage("Price is required.")
				.GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value.");

			RuleFor(x => x.OriginalPrice)
				.NotEmpty().WithMessage("Original Price is required.")
				.GreaterThanOrEqualTo(0).WithMessage("Original Price must be a positive value.");

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Product Name is required.")
				.MaximumLength(200).WithMessage("Product Name cannot exceed 200 characters.");

			RuleFor(x => x.Description)
				.MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

			RuleFor(x => x.SeoDescription)
				.MaximumLength(500).WithMessage("SEO Description cannot exceed 500 characters.");

			RuleFor(x => x.SeoTitle)
				.MaximumLength(100).WithMessage("SEO Title cannot exceed 100 characters.");

			RuleFor(x => x.SeoAlias)
				.NotEmpty().WithMessage("SEO Alias is required.");
		}
	}
}
