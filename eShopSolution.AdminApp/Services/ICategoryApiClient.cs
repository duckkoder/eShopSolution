using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.Catalog.Categories;

namespace eShopSolution.AdminApp.Services
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryViewModel>> GetAll(string languageId);
    }
}
