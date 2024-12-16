using APIServices;
using eShopSolution.Utilities.Constants;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApp.ViewComponents
{
    public class SlideViewComponent : ViewComponent
    {
        private readonly IBrandApiClient _brandApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        public SlideViewComponent(IBrandApiClient brandApiClient, ICategoryApiClient categoryApiClient) {
            _brandApiClient = brandApiClient;
            _categoryApiClient = categoryApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID);
            var brands = await _brandApiClient.GetAll();
            var categories = await _categoryApiClient.GetAll(languageId);

            var model = new SlideViewModel {
                Brands = brands.ResultObj,
                Categories = categories.ResultObj
            };

            return View("Default",model);
        }
    }
}
