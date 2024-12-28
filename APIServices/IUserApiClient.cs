using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;

namespace APIServices
{
    public interface IUserApiClient
    {
        public Task<ApiResult<string>> Authenticate(LoginRequest request);
        public Task<ApiResult<string>> AuthenticateWithGoogle(LoginWithGoogleRequest request);

        public Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);

        public Task<ApiResult<bool>> RegisterUser(RegisterRequest request);

        public Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);

        public Task<ApiResult<UserViewModel>> GetById(Guid id);

        public Task<ApiResult<bool>> DeleteUser(Guid id);

        public Task<ApiResult<bool>> RoleAssignUser(Guid id, RoleAssignRequest request);

    }
}
