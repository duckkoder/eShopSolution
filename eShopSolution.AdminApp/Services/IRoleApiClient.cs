using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;

namespace eShopSolution.AdminApp.Services
{
    public interface IRoleApiClient
    {
        public Task<ApiResult<List<RoleViewModel>>> GetAll();
    }
}
