using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Language;

namespace APIServices
{
    public interface ILanguageApiClient
    {
        Task<ApiResult<List<LanguageViewModel>>> GetAll();
    }
}
