using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;

namespace eShopSolution.AdminApp.Services
{
    public class UserApiClient : ApiClientBase,IUserApiClient
    {
        public UserApiClient(IHttpClientFactory httpClientFactory,
                                 IConfiguration configuration,
                                 IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {

        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await PostAsync<ApiResult<string>>("/api/users/authenticate", httpContent);   
        }

        public async Task<ApiResult<bool>> DeleteUser(Guid id)
        {
            return await DeleteAsync<ApiResult<bool>>($"/api/users/{id}");
            
        }

        public async Task<ApiResult<UserVM>> GetById(Guid id)
        {
            return  await GetAsync<ApiResult<UserVM>>($"/api/users/{id}");
        }

        public async Task<ApiResult<PagedResult<UserVM>>> GetUserPaging(GetUserPagingRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);
            string keywordParam = string.IsNullOrWhiteSpace(request.Keyword) ? string.Empty : $"&keyword={request.Keyword}";
            return await GetAsync<ApiResult<PagedResult<UserVM>>>
                ($"/api/users/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}" +
                $"{keywordParam}&BearerToken={sessions}");
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await PostAsync<ApiResult<bool>>("/api/users", httpContent);
        }

        public async Task<ApiResult<bool>> RoleAssignUser(Guid id, RoleAssignRequest request)
        {

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await PutAsync<ApiResult<bool>>($"/api/users/{id}/roles", httpContent);
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return  await PutAsync<ApiResult<bool>>($"/api/users/{id}", httpContent);
        }
    }
}
