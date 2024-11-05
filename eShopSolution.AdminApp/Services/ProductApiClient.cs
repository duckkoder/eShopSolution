using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Newtonsoft.Json;
using System.Text;

namespace eShopSolution.AdminApp.Services
{
    public class ProductApiClient : ApiClientBase,IProductApiClient
    {

        public ProductApiClient(IHttpClientFactory httpClientFactory,IConfiguration configuration,IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory,configuration,httpContextAccessor) { 
        
        }
        public async Task<ApiResult<int>> AddImage(int productId, ProductImageCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await PostAsync<ApiResult<int>>($"api/products/{productId}/image", httpContent);
        }

        public async Task<ApiResult<bool>> AddViewcount(int productId)
        {
            return await PutAsync<ApiResult<bool>>($"api/products/ViewCount");
        }

        public async Task<ApiResult<int>> Create(ProductCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await PostAsync<ApiResult<int>>($"api/products", httpContent);
        }

        public Task<ApiResult<bool>> Delete(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<List<ProductViewModel>>> GetAll(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<PagedResult<ProductViewModel>>> GetAllByCatagoryId(string languageId, GetPublicProductPagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<PagedResult<ProductViewModel>>> GetAllPaging(GetManageProductPagingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<ProductViewModel>> GetById(int productId, string languageId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<ProductImageViewModel>> GetImageById(int imageId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<List<ProductImageViewModel>>> GetListImages(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> RemoveImage(int imageId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> Update(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdatePrice(int productId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateStock(int productId, int addedQuantity)
        {
            throw new NotImplementedException();
        }
    }
}
