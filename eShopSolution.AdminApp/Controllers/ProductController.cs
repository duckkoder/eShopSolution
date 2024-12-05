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
        private readonly IBrandApiClient _brandApiClient;
        private readonly ICategoryApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient,
            IConfiguration configuration, IBrandApiClient brandApiClient, ICategoryApiClient categoryApiClient)
        {
            _configuration = configuration;
            _productApiClient = productApiClient;
            _brandApiClient = brandApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);

            var result = await _productApiClient.GetAll(languageId);

            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _brandApiClient.GetAll();

            if (!result.IsSuccessed)
                return BadRequest(result);

            ViewBag.Brands = result.ResultObj;

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
                    Name = product.Name,
                    BrandId = product.BrandId,
                    Details = product.Details,  
                    Id = product.Id,
                    LanguageId = languageId,
                    SeoAlias = product.SeoAlias,
                    SeoDescription = product.SeoDescription,
                    SeoTitle = product.SeoTitle,
                    OriginalPrice = product.OriginalPrice,
                    Price = product.Price,
                };
                return View(request);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [Consumes("multipart/form-data")]

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

        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int id)
        {
            var categoryAssignRequest = await GetCategoryAssignRequest(id);
            return View(categoryAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.CategoryAssign(request.Id, request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetCategoryAssignRequest(request.Id);

            return View(roleAssignRequest);
        }

        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);

            var productObj = await _productApiClient.GetById(id, languageId);
            var categories = await _categoryApiClient.GetAll(languageId);
            var categoryAssignRequest = new CategoryAssignRequest();
            foreach (var c in categories)
            {
                categoryAssignRequest.Categories.Add(new SelectedItem()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    IsSelected = productObj.ResultObj.Categories.Contains(c.Name)
                });
            }
            return categoryAssignRequest;
        }

        [HttpGet]
        public async Task<IActionResult> GetSize(int id)
        {
            var result = await _productApiClient.GetQuantity(id);

            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return RedirectToAction("Index");
            }
            var request = new UpdateQuantityRequest() { ProductId = id ,ProductSizes=result.ResultObj};

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> GetSize(UpdateQuantityRequest request)
        {
            var result = await _productApiClient.UpdateQuantity(request.ProductId,request);

            if (!result.IsSuccessed)
            {
                return RedirectToAction("Error", "Home");
            }
            TempData["message"] = result.Message;
            return RedirectToAction("Index");
        }

    }
}
