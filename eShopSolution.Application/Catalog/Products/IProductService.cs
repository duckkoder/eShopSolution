
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IProductService
	{
		public Task<int> Create(ProductCreateRequest request);

		public Task<int> Update(ProductUpdateRequest request);

		public Task<int> Delete(int productId);

		public Task<ProductViewModel> GetById(int productId, string languageId);

		public Task<bool> UpdatePrice(int productId, decimal newPrice);

		public Task<bool> UpdateStock(int productId, int addedQuantity);

		public Task<int> AddViewcount(int productId);

		public Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

		public Task<int> AddImage(int productId, ProductImageCreateRequest request);
		public Task<int> RemoveImage(int imageId);
		public Task<int> UpdateImage(int imageId,ProductImageUpdateRequest request);

		public Task<ProductImageViewModel> GetImageById(int imageId);
		public Task<List<ProductImageViewModel>> GetListImages(int productId);

        Task<PagedResult<ProductViewModel>> GetAllByCatagoryId(string languageId, GetPublicProductPagingRequest request);

        Task<List<ProductViewModel>> GetAll(string id);

    }
}