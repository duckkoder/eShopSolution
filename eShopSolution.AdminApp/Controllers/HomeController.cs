using eShopSolution.AdminApp.Models;
using eShopSolution.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace eShopSolution.AdminApp.Controllers
{
	public class HomeController : BaseController
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			var user = User.Identity.Name;
            ViewData["Title"] = "User";
            return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public IActionResult Language(NavigationViewModel navigationViewModel)
		{
			HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLanguageID,navigationViewModel.CurrentLanguage);
            return View("Index");
		}
	}
}
