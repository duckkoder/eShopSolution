using eShopSolution.ViewModels.Catalog.Brands;
using eShopSolution.ViewModels.Common;

namespace eShopSolution.Application.Catalog.Brands
{

    public interface  IBrandService
    {
        Task<ApiResult<bool>> Create(BrandCreateRequest request);

        Task<ApiResult<bool>> Delete(int Id);

        Task<ApiResult<bool>> Update(BrandUpdateRequest request);

        Task<ApiResult<BrandViewModel>> GetById(int Id);

        Task<ApiResult<List<BrandViewModel>>> GetAll();

    }
}
