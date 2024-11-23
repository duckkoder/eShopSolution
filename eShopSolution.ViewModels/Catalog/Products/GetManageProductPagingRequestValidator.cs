using FluentValidation;
using eShopSolution.ViewModels.Catalog.Products;

public class GetManageProductPagingRequestValidator : AbstractValidator<GetManageProductPagingRequest>
{
	public GetManageProductPagingRequestValidator()
	{
		// Xác thực Keyword: Có thể là null hoặc không rỗng và không quá 200 ký tự
		RuleFor(x => x.Keyword)
			.MaximumLength(200).WithMessage("Keyword cannot exceed 200 characters.");

		// Xác thực CategoryIds: Nếu có giá trị, danh sách không được rỗng
		RuleFor(x => x.PageIndex)
			.GreaterThanOrEqualTo(1).WithMessage("PageIndex must be greater than or equal to 1.");


		RuleFor(x => x.PageSize)
			.InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
	}
}
