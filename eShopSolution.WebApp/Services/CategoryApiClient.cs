using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Categories;
using System.Collections.Generic;

namespace eShopSolution.AdminApp.Services
{
    public class CategoryApiClient : ApiClientBase, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<List<CategoryViewModel>> GetAll(string languageId)
        {
            return await GetAsync <List< CategoryViewModel>> ("/api/categories?languageId=" + languageId);
        }
    }
}
