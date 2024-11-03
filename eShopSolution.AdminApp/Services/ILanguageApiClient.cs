using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Language;

namespace eShopSolution.AdminApp.Services
{
    public interface ILanguageApiClient 
    {
        Task<ApiResult<List<LanguageVM>>> GetAll();
    }
}
