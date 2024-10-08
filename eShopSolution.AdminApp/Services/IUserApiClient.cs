using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;

namespace eShopSolution.AdminApp.Services
{
    public interface IUserApiClient
    {
        public Task<ApiResult<string>> Authenticate(LoginRequest request);

        public Task<ApiResult<PagedResult<UserVM>>> GetUserPaging(GetUserPagingRequest request);

        public Task<ApiResult<bool>> RegisterUser (RegisterRequest request); 

        public Task<ApiResult<bool>> UpdateUser (Guid id, UserUpdateRequest request);

        public Task<ApiResult<UserVM>> GetById(Guid id);

    }
}
