﻿using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Catalog.ProductSizes;
using eShopSolution.ViewModels.Common;

namespace APIServices
{
    public interface IProductApiClient
    {
        Task<ApiResult<int>> Create(ProductCreateRequest request);

        Task<ApiResult<bool>> Update(ProductUpdateRequest request);

        Task<ApiResult<bool>> Delete(int productId);

        Task<ApiResult<ProductViewModel>> GetById(int productId, string languageId);

        Task<ApiResult<bool>> UpdatePrice(int productId, decimal newPrice);

        Task<ApiResult<bool>> UpdateStock(int productId, int addedQuantity);

        Task<ApiResult<bool>> AddViewcount(int productId);

        Task<ApiResult<PagedResult<ProductViewModel>>> GetAllByKeywordAndCatagoryId(GetManageProductPagingRequest request);

        Task<ApiResult<int>> AddImage(int productId, ProductImageCreateRequest request);
        Task<ApiResult<bool>> RemoveImage(int imageId);
        Task<ApiResult<bool>> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<ApiResult<ProductImageViewModel>> GetImageById(int imageId);
        Task<ApiResult<List<ProductImageViewModel>>> GetListImages(int productId);

        Task<ApiResult<PagedResult<ProductViewModel>>> GetAllByCatagoryId(string languageId, GetPublicProductPagingRequest request);

        Task<ApiResult<List<ProductViewModel>>> GetAll(string id);
        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
        Task<ApiResult<List<ProductSizeViewModel>>> GetQuantity(int id);
        Task<ApiResult<bool>> UpdateQuantity(int id, UpdateQuantityRequest request);
        Task<ApiResult<List<ProductViewModel>>> GetProductByBrand(int brandId,string languageId);
        Task<ApiResult<string>> ProductSizePredict(ProductSizePredictRequest request);


    }
}
