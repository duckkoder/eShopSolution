using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;

namespace eShopSolution.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);

        Task<ApiResult<bool>> Update(Guid userId, UserUpdateRequest requestt);

        Task<ApiResult<UserViewModel>> GetById(Guid id);
        Task<ApiResult<bool>> Delete(Guid id);
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);

    }
}
