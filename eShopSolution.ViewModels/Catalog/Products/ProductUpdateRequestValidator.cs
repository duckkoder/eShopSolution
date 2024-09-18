using FluentValidation;
using eShopSolution.ViewModels.Catalog.Products;

public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
	public ProductUpdateRequestValidator()
	{
		// Xác thực Id: phải lớn hơn 0
		RuleFor(x => x.Id)
			.GreaterThan(0).WithMessage("Product Id must be greater than 0.");

		// Xác thực Name: không được để trống và không vượt quá 200 ký tự
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Product Name is required.")
			.MaximumLength(200).WithMessage("Product Name cannot exceed 200 characters.");

		// Xác thực Description: không vượt quá 500 ký tự
		RuleFor(x => x.Description)
			.MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

		// Xác thực SeoDescription: không vượt quá 500 ký tự
		RuleFor(x => x.SeoDescription)
			.MaximumLength(500).WithMessage("SEO Description cannot exceed 500 characters.");

		// Xác thực SeoTitle: không vượt quá 100 ký tự
		RuleFor(x => x.SeoTitle)
			.MaximumLength(100).WithMessage("SEO Title cannot exceed 100 characters.");

		// Xác thực SeoAlias: không được để trống
		RuleFor(x => x.SeoAlias)
			.NotEmpty().WithMessage("SEO Alias is required.");

		// Xác thực LanguageId: không được để trống
		RuleFor(x => x.LanguageId)
			.NotEmpty().WithMessage("Language ID is required.");

		// Xác thực ThumbnailImage: có thể null, nhưng nếu có thì phải là file hợp lệ
		RuleFor(x => x.ThumbnailImage)
			.Must(file => file == null || file.Length > 0)
			.WithMessage("Thumbnail Image must be a valid file if provided.");
	}
}
