using eShopSolution.Application.Catalog.Carts;
using eShopSolution.ViewModels.Catalog.Carts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService) { 
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProductToCart(AddToCartRequest request)
        {
            var result = await _cartService.AddProductToCart(request);
            if(!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("{userId}/{languageId}")]
        public async Task<IActionResult> GetCartByUserID(Guid userId, string languageId)
        {
            var result = await _cartService.GetCartByUserID(userId,languageId);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateCart(UpdateCartRequest request)
        {
            var result = await _cartService.UpdateCart(request);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
