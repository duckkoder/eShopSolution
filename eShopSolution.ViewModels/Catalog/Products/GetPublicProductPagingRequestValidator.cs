using FluentValidation;
using eShopSolution.ViewModels.Catalog.Products;

public class GetPublicProductPagingRequestValidator : AbstractValidator<GetPublicProductPagingRequest>
{
	public GetPublicProductPagingRequestValidator()
	{
		// Xác thực CategoryId: Nếu có giá trị, phải lớn hơn 0
		RuleFor(x => x.CategoryId)
			.GreaterThan(0).When(x => x.CategoryId.HasValue)
			.WithMessage("CategoryId must be greater than 0 if provided.");

		// Xác thực các thuộc tính phân trang (PageIndex, PageSize)
		RuleFor(x => x.PageIndex)
			.GreaterThanOrEqualTo(1).WithMessage("PageIndex must be greater than or equal to 1.");

		RuleFor(x => x.PageSize)
			.InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
	}
}
