using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace eShopSolution.AdminApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IRoleApiClient _roleApiClient;
        private readonly IConfiguration _configuration;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _roleApiClient = roleApiClient;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var sessions = HttpContext.Session.GetString("Token");

            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetUserPaging(request);
            if (TempData["message"]!=null)
                ViewBag.message = TempData["message"] as string;
            return View(data.ResultObj);
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = id
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.UpdateUser(request.Id, request);

            if (result.IsSuccessed)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetById(id);

            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return RedirectToAction("Index");
            }
            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userApiClient.GetById(id);

            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return RedirectToAction("Index");
            }

            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserViewModel user)
        {
            var result = await _userApiClient.DeleteUser(user.Id);

            if (!result.IsSuccessed)
            {
                return RedirectToAction("Error", "Home");
            }
            TempData["message"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            roleAssignRequest.id = id;
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RoleAssignUser(request.id, request);

            if (result.IsSuccessed)
            {
                TempData["message"] = result.Message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetRoleAssignRequest(request.id);

            return View(roleAssignRequest);
        }

        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetById(id);
            var roleObj = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
          
            foreach (var role in roleObj.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectedItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    IsSelected = userObj.ResultObj.Roles.Contains(role.Name),
                    Description = role.Description
                });
            }
            return roleAssignRequest;
        }
    }
}
