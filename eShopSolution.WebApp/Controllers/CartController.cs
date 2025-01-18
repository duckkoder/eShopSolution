using APIServices;
using Azure.Core;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Carts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;

namespace eShopSolution.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartApiClient _cartApiClient;
        public CartController(ICartApiClient cartApiClient) { 
            _cartApiClient = cartApiClient;
        }

       


        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartRequest request)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid userIdGuid = Guid.Parse(userId);

            request.Id = userIdGuid;
            var result = await _cartApiClient.AddProductToCart(request);

            if (result.IsSuccessed)
            {
                return Json(new { success = true, message = "Product added to cart successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to add product to cart." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["message"] = "Login To Order <3";
                return RedirectToAction("Login","Account");
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid Id = Guid.Parse(userId);
            var currentLanguage = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);
            var result = await _cartApiClient.GetCartByUserID(Id,currentLanguage);
            if (result.IsSuccessed)
            {
                return View(result.ResultObj);
            }
            ModelState.AddModelError("massage", result.Message);
            return View(result.ResultObj);  
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(UpdateCartRequest request)
        {
            var result = await _cartApiClient.UpdateCart(request);

            if (result.IsSuccessed)
            {
                return Json(new { success = true, message = "Update cart successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to update cart." });
            }
        }


    }
}
