using APIServices;

namespace eShopSolution.WebApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient();

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddTransient<IUserApiClient, UserApiClient>();
            builder.Services.AddTransient<IRoleApiClient, RoleApiClient>();
            builder.Services.AddTransient<ILanguageApiClient, LanguageApiClient>();
            builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
            builder.Services.AddTransient<IBrandApiClient, BrandApiClient>();
            builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();


            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            //use HttpContextAccessor 
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


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
