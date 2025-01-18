using eShopSolution.Application.Catalog.Brands;
using eShopSolution.Application.Catalog.Carts;
using eShopSolution.Application.Catalog.Categories;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Application.Common;
using eShopSolution.Application.System.Languages;
using eShopSolution.Application.System.Roles;
using eShopSolution.Application.System.Users;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


namespace eShopSolution.BackendApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<EShopDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));




			

			builder.Services.AddIdentity<AppUser, AppRole>()
				.AddEntityFrameworkStores<EShopDbContext>()
				.AddDefaultTokenProviders();

			//Declare Dependency injection (DI) 
			builder.Services.AddTransient<IStorageService, FileStorageService>();
			builder.Services.AddTransient<IProductService, ProductService>();
			builder.Services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
			builder.Services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
			builder.Services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
			builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IRoleService, RoleService>();
			builder.Services.AddTransient<ILanguageService, LanguageService>();
            builder.Services.AddTransient<IBrandService, BrandService>();
			builder.Services.AddTransient<ICategoryService, CategoryService>();
			builder.Services.AddTransient<ICartService, CartService>();



            //Catalog DI
            builder.Services.AddTransient<IValidator<ProductImageCreateRequest>, ProductImageCreateRequestValidator>();
			builder.Services.AddTransient<IValidator<ProductImageUpdateRequest>, ProductImageUpdateRequestValidator>();

			builder.Services.AddTransient<IValidator<GetManageProductPagingRequest>, GetManageProductPagingRequestValidator>();
			builder.Services.AddTransient<IValidator<GetPublicProductPagingRequest>, GetPublicProductPagingRequestValidator>();
			builder.Services.AddTransient<IValidator<ProductCreateRequest>, ProductCreateRequestValidator>();
			builder.Services.AddTransient<IValidator<ProductUpdateRequest>, ProductUpdateRequestValidator>();

			builder.Services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
			builder.Services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidation>();


            builder.Services.AddControllers()
				.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());


			builder.Services.AddSwaggerGen(c =>
			{
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger eShop Solution", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                    });
            });

			string issuer = builder.Configuration.GetValue<string>("Tokens:Issuer");
			string signingKey = builder.Configuration.GetValue<string>("Tokens:Key");
			byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

			builder.Services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = issuer,
					ValidateAudience = true,
					ValidAudience = issuer,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ClockSkew = System.TimeSpan.Zero,
					IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
				};
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

			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger eShopSolution V1"));


			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
