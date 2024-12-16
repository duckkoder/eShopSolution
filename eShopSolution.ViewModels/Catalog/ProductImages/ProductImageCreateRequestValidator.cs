using FluentValidation;
using eShopSolution.ViewModels.Catalog.ProductImages;

public class ProductImageCreateRequestValidator : AbstractValidator<ProductImageCreateRequest>
{
	public ProductImageCreateRequestValidator()
	{
		// Xác thực Caption: không được để trống và không vượt quá 200 ký tự
		RuleFor(x => x.Caption)

			.MaximumLength(200).WithMessage("Caption cannot exceed 200 characters.");

		// Xác thực SortOrder: phải là số nguyên dương hoặc 0
		RuleFor(x => x.SortOrder)
			.GreaterThanOrEqualTo(0).WithMessage("Sort Order must be a non-negative integer.");

		// Xác thực ImageFile: phải là file hợp lệ và không được rỗng
		RuleFor(x => x.ImageFile)
			.NotNull().WithMessage("ImageFile is required.");
			

		// Xác thực IsDefault: không cần kiểm tra cụ thể, vì đây là giá trị boolean
	}
}
