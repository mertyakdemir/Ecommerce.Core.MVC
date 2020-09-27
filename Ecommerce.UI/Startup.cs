using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.BusinessLayer.Abstract;
using Ecommerce.BusinessLayer.Concrete;
using Ecommerce.DataLayer;
using Ecommerce.DataLayer.Abstract;
using Ecommerce.DataLayer.Concrete;
using Ecommerce.UI.Email;
using Ecommerce.UI.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ecommerce.UI
{
    public class Startup
    {

        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IAppContext>(options => options.UseSqlServer
            ("Server=TEST\\SQLEXPRESS;Database=TestDatabase; Integrated Security=SSPI; MultipleActiveResultSets=True;"));

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IAppContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".Ecommerce.Cookie"
                };
            });

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryBusiness, CategoryManager>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductBusiness, ProductManager>();

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartBusiness, CartManager>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderBusiness, OrderManager>();

            services.AddControllersWithViews();

            services.AddScoped<IEmail, GmailStmp>(i => new GmailStmp(
                _configuration["EmailStmp:Host"],
                _configuration.GetValue<int>("EmailStmp:Port"),
                _configuration.GetValue<bool>("EmailStmp:EnableSSL"),
                _configuration["EmailStmp:Email"],
                _configuration["EmailStmp:Password"]
                ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                Initializer.Seed();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
             
                endpoints.MapControllerRoute(
                   name: "adminproducts",
                   pattern: "admin/products",
                   defaults: new { controller = "Admin", action = "ProductList" }
                   );

                endpoints.MapControllerRoute(
                   name: "adminproductscreate",
                   pattern: "admin/products/create",
                   defaults: new { controller = "Admin", action = "CreateProduct" }
                   );

                endpoints.MapControllerRoute(
                   name: "adminproductsedit",
                   pattern: "admin/products/{id?}",
                   defaults: new { controller = "Admin", action = "EditProduct" }
                   );

                endpoints.MapControllerRoute(
                   name: "admincategories",
                   pattern: "admin/categories",
                   defaults: new { controller = "Admin", action = "CategoryList" }
                   );

                endpoints.MapControllerRoute(
                  name: "admincategorycreate",
                  pattern: "admin/categories/create",
                  defaults: new { controller = "Admin", action = "CreateCategory" }
                  );

                endpoints.MapControllerRoute(
                   name: "admincategoryedit",
                   pattern: "admin/categories/{id?}",
                   defaults: new { controller = "Admin", action = "EditCategory" }
                   );

                endpoints.MapControllerRoute(
                  name: "cart",
                  pattern: "cart",
                  defaults: new { controller = "Cart", action = "Index" }
                  );

                endpoints.MapControllerRoute(
                  name: "checkout",
                  pattern: "checkout",
                  defaults: new { controller = "Cart", action = "Checkout" }
                  );

                endpoints.MapControllerRoute(
                  name: "orders",
                  pattern: "orders",
                  defaults: new { controller = "Cart", action = "Orders" }
                  );

                endpoints.MapControllerRoute(
                    name: "search",
                    pattern: "search",
                    defaults: new { controller = "Store", action = "Search" }
                    );

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "products/{category?}",
                    defaults: new {controller= "Store", action= "Index"}
                    );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
