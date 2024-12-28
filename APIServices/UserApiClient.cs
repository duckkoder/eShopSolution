using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace APIServices
{
    public class UserApiClient : ApiClientBase, IUserApiClient
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

        public async Task<ApiResult<string>> AuthenticateWithGoogle(LoginWithGoogleRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await PostAsync<ApiResult<string>>("/api/users/authenticate-with-google", httpContent);
        }

        public async Task<ApiResult<bool>> DeleteUser(Guid id)
        {
            return await DeleteAsync<ApiResult<bool>>($"/api/users/{id}");

        }

        public async Task<ApiResult<UserViewModel>> GetById(Guid id)
        {
            return await GetAsync<ApiResult<UserViewModel>>($"/api/users/{id}");
        }

        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await PostAsync<ApiResult<PagedResult<UserViewModel>>>
                ($"/api/users/paging", httpContent);
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
            return await PutAsync<ApiResult<bool>>($"/api/users/{id}", httpContent);
        }
    }
}
