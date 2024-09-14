using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eShopSolution.Application.System.Users
{
	public class UserService : IUserService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly IConfiguration _config;
		public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_config = configuration;
		}
		public async Task<string> Authenticate(LoginRequest request)
		{
			var user = await _userManager.FindByNameAsync(request.UserName);
			if (user == null) return null;

			var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.IsRememberMe, true);
			if (!result.Succeeded)
			{
				return null;
			}
			var roles = await _userManager.GetRolesAsync(user);
			var claims = new[]
			{
				new Claim(ClaimTypes.Email,user.Email),
				new Claim(ClaimTypes.GivenName,user.FirstName),
				new Claim(ClaimTypes.Role, string.Join(";",roles))
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(_config["Tokens:Issuer"],
				_config["Tokens:Issuer"],
				claims,
				expires: DateTime.Now.AddHours(3),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<bool> Register(RegisterRequest request)
		{
			var user = new AppUser()
			{
				UserName = request.UserName,
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				PhoneNumber = request.PhoneNumber,
				Dob = request.Dob,
			};

			var result = await _userManager.CreateAsync(user,request.Password);

			if (result.Succeeded)
				return true;
			return false;
		}
	}
}
