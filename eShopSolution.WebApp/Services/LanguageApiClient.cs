using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Language;
using eShopSolution.ViewModels.System.Roles;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace eShopSolution.AdminApp.Services
{
    public class LanguageApiClient : ApiClientBase, ILanguageApiClient
    {
        public LanguageApiClient(IHttpClientFactory httpClientFactory,
                                 IConfiguration configuration, 
                                 IHttpContextAccessor httpContextAccessor) 
            : base(httpClientFactory, configuration, httpContextAccessor) {
        
        }


    public async Task<ApiResult<List<LanguageViewModel>>> GetAll()
        {
            string url = "/api/languages";
            return await GetAsync<ApiResult<List<LanguageViewModel>>>(url);

        }
    }
}
