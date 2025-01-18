using eShopSolution.ViewModels.Catalog.Carts;
using eShopSolution.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices
{
    public interface ICartApiClient
    {
        Task<ApiResult<bool>> AddProductToCart(AddToCartRequest request);
        Task<ApiResult<CartViewModel>> GetCartByUserID(Guid Id, string languageId);
        Task<ApiResult<bool>> UpdateCart(UpdateCartRequest request);
    }
}
