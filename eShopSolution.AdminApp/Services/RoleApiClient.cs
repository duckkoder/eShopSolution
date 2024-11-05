using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace eShopSolution.AdminApp.Services
{
    public class RoleApiClient : ApiClientBase, IRoleApiClient
    {
        public RoleApiClient(IHttpClientFactory httpClientFactory, 
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory,configuration,httpContextAccessor) { 
        
        }
        
        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {     
            return await GetAsync<ApiResult<List<RoleViewModel>>>($"/api/roles");
        }
    }
}
