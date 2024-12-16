using APIServices;
using eShopSolution.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApp.Controllers
{
    public class BrandController : Controller
    {
        private readonly IProductApiClient _productApiClient;

        public BrandController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int Id,string name)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);

            var products = await _productApiClient.GetProductByBrand(Id,languageId);

            if (!products.IsSuccessed) {
                return View(products);
            }
            ViewBag.Name = name;

            return View(products.ResultObj);
        }
    }
}
