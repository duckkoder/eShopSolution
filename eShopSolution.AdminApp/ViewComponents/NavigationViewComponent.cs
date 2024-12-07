using eShopSolution.AdminApp.Models;
using APIServices;
using eShopSolution.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace eShopSolution.AdminApp.ViewComponents
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ILanguageApiClient _languageApiClient;

        public NavigationViewComponent(ILanguageApiClient languageApiClient) {
            _languageApiClient = languageApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _languageApiClient.GetAll();
            
            var navigationVM = new NavigationViewModel()
            {
                    Languages = result.ResultObj,
                    CurrentLanguage = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageID)
            };
            
            return View("Default" , navigationVM);
        }
    }
}
