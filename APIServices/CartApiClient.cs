using eShopSolution.ViewModels.Catalog.Carts;
using eShopSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServices
{

    public class CartApiClient : ApiClientBase, ICartApiClient
    {
        public CartApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
           : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> AddProductToCart(AddToCartRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await PostAsync<ApiResult<bool>>($"/api/carts/add",httpContent);
        }

        public async Task<ApiResult<CartViewModel>> GetCartByUserID(Guid Id, string languageId)
        {
            return await GetAsync<ApiResult<CartViewModel>>($"/api/carts/{Id.ToString()}/{languageId}");
        }

        public async Task<ApiResult<bool>> UpdateCart(UpdateCartRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return await PostAsync<ApiResult<bool>>($"/api/carts/update", httpContent); 
        }
    }
}
