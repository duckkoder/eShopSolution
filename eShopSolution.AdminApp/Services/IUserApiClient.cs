using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;

namespace eShopSolution.AdminApp.Services
{
    public interface IUserApiClient
    {
        public Task<string> Authenticate(LoginRequest request);

        public Task<PagedResult<UserVM>> GetUserPaging(GetUserPagingRequest request);
    }
}
