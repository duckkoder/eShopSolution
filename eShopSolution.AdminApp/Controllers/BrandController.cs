using APIServices;
using eShopSolution.ViewModels.Catalog.Brands;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.AdminApp.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandApiClient _brandApiClient;

        public BrandController(IBrandApiClient brandApiClient)
        {
            _brandApiClient = brandApiClient;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _brandApiClient.GetAll();
            if (!result.IsSuccessed) {
                ModelState.AddModelError("",result.Message);
                return View(ModelState);
            }
            return View(result.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(BrandCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _brandApiClient.Create(request);
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
            var result = await _brandApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var brand = result.ResultObj;
                var request = new BrandUpdateRequest()
                {
                    Name = brand.Name,
                    Id = id,
                    Description = brand.Description,
                };
                return View(request);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(BrandUpdateRequest request)
        {
            if(!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _brandApiClient.Update(request);
            if (result.IsSuccessed)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("",result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _brandApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                return View(result.ResultObj);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BrandViewModel brand)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _brandApiClient.Delete(brand.Id);
            if (result.IsSuccessed)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(brand);
        }



    }
}
