using FluentValidation;
using eShopSolution.ViewModels.Catalog.ProductImages;

public class ProductImageUpdateRequestValidator : AbstractValidator<ProductImageUpdateRequest>
{
	public ProductImageUpdateRequestValidator()
	{
		// Xác thực Caption: không được để trống và không vượt quá 200 ký tự
		RuleFor(x => x.Caption)
			.NotEmpty().WithMessage("Caption is required.")
			.MaximumLength(200).WithMessage("Caption cannot exceed 200 characters.");

		// Xác thực SortOrder: phải là số nguyên dương hoặc 0
		RuleFor(x => x.SortOrder)
			.GreaterThanOrEqualTo(0).WithMessage("Sort Order must be a non-negative integer.");

		// Xác thực ImageFile: nếu có file thì phải hợp lệ
		RuleFor(x => x.ImageFile)
			.Must(file => file == null || file.Length > 0).WithMessage("ImageFile must be a valid file if provided.");

		// Xác thực IsDefault: không cần kiểm tra vì là boolean
	}
}
