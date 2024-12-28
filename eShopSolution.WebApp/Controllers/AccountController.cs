using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using APIServices;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using RestSharp;

namespace eShopSolution.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public AccountController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.Authenticate(request);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            return await CreateLoginSession(result.ResultObj);
        }


        
        private async Task<IActionResult> CreateLoginSession(string Token)
        {
            
            var userPrincipal = this.ValidateToken(Token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLanguageID, _configuration["DefaultLanguageID"]);
            HttpContext.Session.SetString(SystemConstants.AppSettings.Token, Token);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "Account");
        }

        


        [HttpGet]
        public IActionResult Register(RegisterRequest? request)
        {
            if(request.Email != null)
            {
                ViewData["Type"] = "Google";
                return View(request);
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Register","Account",request);

            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                TempData["message"] = "Create Success, Login To Use Our Services!";
                return RedirectToAction("Login", "Account");
            }
            TempData["message"] = result.Message;
            return RedirectToAction("Register", "Account", request);

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

            return principal;
        }


        public async Task<IActionResult> GoogleLoginCallback(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    // Lấy access token từ Google
                    var token = GetGoogleAccessToken(code);
                    if (string.IsNullOrEmpty(token))
                    {
                        ViewBag.message = "Không thể lấy access token từ Google.";
                        return View("Login","Account");
                    }

                    // Lấy thông tin người dùng
                    var userInfo = GetGoogleUserInfo(token);
                    if (userInfo == null)
                    {
                        ViewBag.message = "Không thể lấy thông tin người dùng.";
                        return View("Login", "Account");
                    }

                    //check đã login bằng email chưa?
                    var isLoggedInBefore = await _userApiClient.AuthenticateWithGoogle(new LoginWithGoogleRequest() { Email = userInfo["email"].ToString() }) ;  
                    if(isLoggedInBefore.IsSuccessed) {
                        //TempData["message"] = isLoggedInBefore.Message;
                        return await CreateLoginSession(isLoggedInBefore.ResultObj);
                    }

                    var registerRequest = new RegisterRequest()
                    {
                        FirstName = userInfo["given_name"]?.ToString(),
                        LastName = userInfo["family_name"]?.ToString(),
                        Email = userInfo["email"]?.ToString(),
                        IsEmailConfirmed = true
                    };
                    TempData["message"] = "Please fill in the information to complete registration!";
                    return RedirectToAction("Register", "Account",registerRequest);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                    ViewBag.message = "Đã xảy ra lỗi trong quá trình xử lý.";
                    return View("Login", "Account");
                }
            }

            ViewBag.message = "Mã xác thực không hợp lệ.";
            return View("Login","Account");

        }

        private string GetGoogleAccessToken(string code)
        {
            var client = new RestClient("https://www.googleapis.com/oauth2/v4/token");
            var request = new RestRequest() { Method = Method.Post };
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", "https://localhost:7072/Account/GoogleLoginCallback");
            request.AddParameter("client_id", _configuration["Authentication:Google:ClientId"]);
            request.AddParameter("client_secret", _configuration["Authentication:Google:ClientSecret"]);

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var content = response.Content;
                var res = JsonConvert.DeserializeObject<JObject>(content);
                return res?["access_token"]?.ToString();
            }

            Console.WriteLine("Không thể lấy access token: " + response.ErrorMessage);
            return null;
        }

        private JObject GetGoogleUserInfo(string accessToken)
        {
            var client = new RestClient("https://www.googleapis.com/oauth2/v1/userinfo?alt=json");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest() { Method = Method.Get };

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var content = response.Content;
                return JsonConvert.DeserializeObject<JObject>(content);
            }

            Console.WriteLine("Không thể lấy thông tin người dùng: " + response.ErrorMessage);
            return null;
        }

    }
}
