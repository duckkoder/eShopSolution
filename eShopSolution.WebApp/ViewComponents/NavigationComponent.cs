using APIServices;
using eShopSolution.Utilities.Constants;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.WebApp.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly IBrandApiClient _brandApiClient;
        private readonly ICategoryApiClient _categoryApiClient;


        public NavigationViewComponent(IBrandApiClient brandApiClient, ICategoryApiClient categoryApiClient)
        {
            _brandApiClient = brandApiClient;
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languageId = "en-US";

            var brands = await _brandApiClient.GetAll();
            var categories = await _categoryApiClient.GetAll(languageId);

            var model = new NavigationViewModel() { 
                Brands = brands.ResultObj,
                Categories = categories.ResultObj
            };

            return View("Default", model);
        }
    }
}
