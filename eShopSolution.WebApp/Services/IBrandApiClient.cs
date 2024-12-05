using eShopSolution.ViewModels.Catalog.Brands;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.AdminApp.Services
{
    public interface IBrandApiClient 
    {
        Task<ApiResult<bool>> Create(BrandCreateRequest request);
        Task<ApiResult<bool>> Update(BrandUpdateRequest request);
        Task<ApiResult<bool>> Delete(int Id);
        Task<ApiResult<List<BrandViewModel>>> GetAll();
        Task<ApiResult<BrandViewModel>> GetById(int Id);


    }
}
