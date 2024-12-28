using APIServices;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Users;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using System.Configuration;

namespace eShopSolution.WebApp
{
    public class Program
    {

        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            builder.Services.AddHttpClient();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                option =>
                {
                    option.LoginPath = "/Account/Login/";
                    option.AccessDeniedPath = "/Account/Forbidden/";
                }
                );

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //use HttpContextAccessor 
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            builder.Services.AddTransient<IUserApiClient, UserApiClient>();
            builder.Services.AddTransient<IRoleApiClient, RoleApiClient>();
            builder.Services.AddTransient<ILanguageApiClient, LanguageApiClient>();
            builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
            builder.Services.AddTransient<IBrandApiClient, BrandApiClient>();
            builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();

            builder.Services.AddControllersWithViews()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            });
          
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
     


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

//callback path

