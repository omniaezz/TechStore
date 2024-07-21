using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using System.Globalization;
using TechStore.Application.Contract;
using TechStore.Application.Services;
using TechStore.Context;
using TechStore.Dtos.UserDTO;
using TechStore.Infrastructure;
using TechStore.Models;

namespace TechStore.ViewUser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<TechStoreContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
            builder.Services.AddIdentity<TechUser, IdentityRole>().AddEntityFrameworkStores<TechStoreContext>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategorySpecificationsRepository, CategorySpecificationsRepository>();
            builder.Services.AddScoped<IProductService, Application.Services.ProductService>();//ambigous
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IproductCategorySpecifications,ProductCategorySpecificationsRepository>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IReviewService, ReviewServices>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IspecificationsRepository, SpecificationsRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Inside ConfigureServices method in Startup.cs
            // Add localization services

            ///-------------------------
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.AddControllersWithViews()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            builder.Services.AddSession();

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                     var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ar-EG"), 
                };
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

           

            ///-----------------------------

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "TechStore.Session";
                options.IdleTimeout = TimeSpan.FromDays(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.MaxAge = TimeSpan.FromDays(30);
            });


       
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            var options = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

           

            app.UseRouting();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
           

            app.Run();
        }
    }
}