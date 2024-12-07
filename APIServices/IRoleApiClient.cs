using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Roles;

namespace APIServices
{
    public interface IRoleApiClient
    {
        public Task<ApiResult<List<RoleViewModel>>> GetAll();
    }
}
