using Azure;
using Azure.Core;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Policy;
using System.Text;

namespace eShopSolution.AdminApp.Services
{
    public class ProductApiClient : ApiClientBase,IProductApiClient
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public ProductApiClient(IHttpClientFactory httpClientFactory,IConfiguration configuration,IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory,configuration,httpContextAccessor) { 
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<ApiResult<int>> AddImage(int productId, ProductImageCreateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await PostAsync<ApiResult<int>>($"/api/products/{productId}/image", httpContent);
        }

        public async Task<ApiResult<bool>> AddViewcount(int productId)
        {
            return await PutAsync<ApiResult<bool>>($"/api/products/ViewCount");
        }

        public async Task<ApiResult<bool>> Create(ProductCreateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);
            var requestContent = new MultipartFormDataContent();
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }

            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalPrice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");

            requestContent.Add(new StringContent(request.Details.ToString()), "details");
            requestContent.Add(new StringContent(request.SeoDescription.ToString()), "seoDescription");
            requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seoTitle");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoAlias");
            requestContent.Add(new StringContent(languageId), "languageId");

            var response = await client.PostAsync($"/api/products/", requestContent);
            
            var result = await response.Content.ReadAsStringAsync();

            
            return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> Delete(int productId)
        {
            return await DeleteAsync<ApiResult<bool>>($"/api/products/{productId}");
        }

        public async Task<ApiResult<List<ProductViewModel>>> GetAll(string id)
        {
            return await GetAsync<ApiResult<List<ProductViewModel>>>($"/api/products/{id}");
        }

        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetAllByCatagoryId(string languageId, GetPublicProductPagingRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await PostAsync<ApiResult<PagedResult<ProductViewModel>>> ($"/api/products/paging/{languageId}", httpContent);
        }

        public async Task<ApiResult<PagedResult<ProductViewModel>>> GetAllByKeywordAndCatagoryId(GetManageProductPagingRequest request)
        {
            var qr = request.Keyword != null ? $"&keyword={request.Keyword}" : "";
            var categoryIdsQuery = request.CategoryIds != null && request.CategoryIds.Any()
                ? $"&{string.Join("&", request.CategoryIds.Select(id => $"CategoryIds={id}"))}"
                : "";

            return await GetAsync<ApiResult<PagedResult<ProductViewModel>>>(
               $"/api/products/paging?pageIndex={request.PageIndex}" +
               $"&pageSize={request.PageSize}" + qr +
               $"&languageId={request.LanguageId}" + categoryIdsQuery);
        }


        public async Task<ApiResult<ProductViewModel>> GetById(int productId, string languageId)
        {
            return await GetAsync<ApiResult<ProductViewModel>>($"/api/products/{productId}/{languageId}");
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

        public async Task<ApiResult<bool>> Update(ProductUpdateRequest request)
        {   
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await PutAsync<ApiResult<bool>>($"/api/products",httpContent);
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
