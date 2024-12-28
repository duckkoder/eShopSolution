using eShopSolution.Utilities.Constants;
using eShopSolution.WebApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eShopSolution.WebApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLanguageID, "en-US");
			var user = User.Identity.Name;
            ViewData["Title"] = "User";
            return View(); 
		}


		
	}
}
