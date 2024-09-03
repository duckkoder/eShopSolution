
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
	public interface IManageProductService
	{
		public Task<int> Create(ProductCreateRequest request);

		public Task<int> Update(ProductUpdateRequest request);

		public Task<int> Delete(int productId);

		public Task<bool> UpdatePrice(int productId, decimal newPrice);

		public Task<bool> UpdateStock(int productId, int addedQuantity);

		public Task AddViewcount(int productId);

		public Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

		public Task<int> AddImages(int productId, List<IFormFile> files);
		public Task<int> RemoveImages(List<IFormFile> files);
		public Task<int> UpdateImages(int imageId, string caption, bool isDefault);
		public Task<List<ProductImageViewModel> > GetListImage(int productId); 

 	}
}