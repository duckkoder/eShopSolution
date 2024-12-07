using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace APIServices
{
    public class CategoryApiClient : ApiClientBase, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<List<CategoryViewModel>>> GetAll(string languageId)
        {
            return await GetAsync<ApiResult<List<CategoryViewModel>>>("/api/categories?languageId=" + languageId);
        }
    }
}
