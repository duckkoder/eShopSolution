using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eShopSolution.AdminApp.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserApiClient _userApiClient;
		private readonly IConfiguration _configuration;
		public UserController(IUserApiClient userApiClient,IConfiguration configuration) { 
			_userApiClient = userApiClient;
			_configuration = configuration;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Login()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginRequest request)
		{
			if(!ModelState.IsValid)
			{
				return View(ModelState);
			}
			var token = await _userApiClient.Authenticate(request);
			var userPrincipal = this.ValidateToken(token);
			var authPoroperties = new AuthenticationProperties
			{
				ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
				IsPersistent = true
			};
			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				userPrincipal,
				authPoroperties
				);

			return Redirect("/");
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login");
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal; // Return the principal after successful validation
        }


    }
}
