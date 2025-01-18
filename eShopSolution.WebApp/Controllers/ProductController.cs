using APIServices;
using Azure.Core;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;

        public ProductController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);

            var product = await _productApiClient.GetById(id, languageId);
            var productSize = await _productApiClient.GetQuantity(id);
            if (!product.IsSuccessed || !productSize.IsSuccessed)
                return NotFound();
            var model = new ProductDetailViewModel()
            {
                product = product.ResultObj,
                ProductSizes = productSize.ResultObj
            };
            return View(model);
        }

    
        [HttpPost]
        public async Task<IActionResult> CalculatePrediction(ProductSizePredictRequest request)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);

            var result = await _productApiClient.ProductSizePredict(request);

            string mess = $"Recommended Size: {result.ResultObj}";
           
            return Content(mess);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductQuickView(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);

            var product = await _productApiClient.GetById(id, languageId);
            var productSize = await _productApiClient.GetQuantity(id);
            if (!product.IsSuccessed || !productSize.IsSuccessed)
                return NotFound();
            var model = new ProductDetailViewModel()
            {
                product = product.ResultObj,
                ProductSizes = productSize.ResultObj
            };

            return PartialView("_QuickViewPartial", model);
        }


    }
}
