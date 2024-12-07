using eShopSolution.ViewModels.Catalog.Categories;
using eShopSolution.ViewModels.Common;

namespace APIServices
{
    public interface ICategoryApiClient
    {
        Task<ApiResult<List<CategoryViewModel>>> GetAll(string languageId);
    }
}
