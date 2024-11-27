using eShopSolution.AdminApp.Services;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Mvc;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;

namespace eShopSolution.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;

        public ProductController(IProductApiClient productApiClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);

            var result = await _productApiClient.GetAll(languageId);

            
            return View(result.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.Create(request);
            if (result.IsSuccessed)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);
            var result = await _productApiClient.GetById(id, languageId);
            if(result.IsSuccessed)
            {
                var product = result.ResultObj;
                var request = new ProductUpdateRequest() { 
                    Description = product.Description,
                    brand = product.brand,
                    Name = product.Name,
                    Details = product.Details,  
                    Id = product.Id,
                    LanguageId = languageId,
                    SeoAlias = product.SeoAlias,
                    SeoDescription = product.SeoDescription,
                    SeoTitle = product.SeoTitle,
                    OriginalPrice = product.OriginalPrice,
                    Price = product.Price,
                    Stock = product.Stock,
                };
                return View(request);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit (ProductUpdateRequest request)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _productApiClient.Update(request);
            if (result.IsSuccessed)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("Index","Product");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);

            var result = await _productApiClient.GetById(id,languageId);

            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return RedirectToAction("Index");
            }

            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductViewModel productViewModel)
        {
            var result = await _productApiClient.Delete(productViewModel.Id);

            if (!result.IsSuccessed)
            {
                return RedirectToAction("Error", "Home");
            }
            TempData["message"] = result.Message;
            return RedirectToAction("Index");
        }


    }
}
