using eShopSolution.ViewModels.Catalog.Brands;
using eShopSolution.ViewModels.Catalog.Carts;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Carts
{
    public interface ICartService
    {
        Task<ApiResult<bool>> AddProductToCart(AddToCartRequest request);
        Task<ApiResult<CartViewModel>> GetCartByUserID(Guid Id,string languageId);
        Task<ApiResult<bool>> UpdateCart(UpdateCartRequest request);
    }
}
