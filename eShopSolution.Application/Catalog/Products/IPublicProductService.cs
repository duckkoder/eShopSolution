using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.Application.Catalog.Products
{
	public interface IPublicProductService
	{
		Task<PagedResult<ProductViewModel>> GetAllByCatagoryId(GetPublicProductPagingRequest request);

		Task<List<ProductViewModel>> GetAll();
	}
}
