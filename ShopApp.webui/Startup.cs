using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopApp.business.Abstract;
using ShopApp.business.Concrete;
using ShopApp.webui.EmailServices;
using ShopApp.webui.Identity;
using ShopAppDataAccess.Abstract;
using ShopAppDataAccess.Concrete.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.webui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql("Host=localhost;Database=shopapp;Username=postgres;Password=kardelen67"));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;

                //lockout
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;


                //options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });
            //server tarafýnýn bizi tanýmasýný saðlar
            //örnðn sayfada gösterilen reklamlarýn aramalarýmaza göre çýkmasý
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/account/login"; //sessioný tanýma yetkigerektiðinde gidilecek alan
                options.LogoutPath = "/account/logout"; //sessiondan çýkma
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true; //sessin def olarak 20 dk bunu true yaparsak her istekte 20 dk tekrar baþlar
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true, //js sciptler ulaamz sadece server
                    Name = ".ShopApp.Security.Cookie",
                    SameSite= SameSiteMode.Strict
                };
            });
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IProductDal, EfCoreProductDal>();

            services.AddScoped<ICartDal, EfCoreCartDal>();
            services.AddScoped<ICartService, CartManager>();

            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<ICategoryDal, EfCoreCategoryDal>();

            services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
                new SmtpEmailSender(Configuration["EmailSender:Host"],
                Configuration.GetValue<int>("EmailSender:Port"),
                Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                Configuration["EmailSender:UserName"],
                Configuration["EmailSender:Password"])
                );
            

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration configuration,UserManager<User> userManager , RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedDatabase.Seed();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            
            app.UseAuthentication();

            app.UseRouting();

           app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "checkout",
                  pattern: "checkout",
                  defaults: new { controller = "Cart", Action = "Checkout" }
                  );
                endpoints.MapControllerRoute(
                  name: "cart",
                  pattern: "cart",
                  defaults: new { controller = "Cart", Action = "Index" }
                  );

                endpoints.MapControllerRoute(
                  name: "adminusers",
                  pattern: "admin/user/list",
                  defaults: new { controller = "Admin", Action = "UserList" }
                  );

                endpoints.MapControllerRoute(
                  name: "adminusersdetail",
                  pattern: "admin/user/{id?}",
                  defaults: new { controller = "Admin", Action = "UserEdit" }
                  );

                endpoints.MapControllerRoute(
                  name: "adminrolesedit",
                  pattern: "admin/role/edit/{id?}",
                  defaults: new { controller = "Admin", Action = "RoleEdit" }
                  );

                endpoints.MapControllerRoute(
                  name: "adminroles",
                  pattern: "admin/role/list",
                  defaults: new { controller = "Admin", Action = "RoleList" }
                  );


                endpoints.MapControllerRoute(
                  name: "adminrolecreate",
                  pattern: "admin/role/create",
                  defaults: new { controller = "Admin", Action = "RoleCreate" }
                  );


                endpoints.MapControllerRoute(
                  name: "admincreatecategory",
                  pattern: "admin/categories/create",
                  defaults: new { controller = "Admin", Action = "CreateCategory" }
                  );

                endpoints.MapControllerRoute(
                   name: "admincategories",
                   pattern: "admin/categories",
                   defaults: new { controller = "Admin", Action = "CategoryList" }
                   );

                endpoints.MapControllerRoute(
                   name: "admineditcategory",
                   pattern: "admin/categories/{id?}",
                   defaults: new { controller = "Admin", Action = "EditCategory" }
                   );


                endpoints.MapControllerRoute(
                   name: "admincreateproduct",
                   pattern: "admin/products/create",
                   defaults: new { controller = "Admin", Action = "CreateProduct" }
                   );

                endpoints.MapControllerRoute(
                   name: "adminproducts",
                   pattern: "admin/products",
                   defaults: new { controller = "Admin", Action = "ProductList" }
                   );

                endpoints.MapControllerRoute(
                   name: "adminEditProduct",
                   pattern: "admin/products/{id?}",
                   defaults: new { controller = "Admin", Action = "EditProduct" }
                   );

                

                endpoints.MapControllerRoute(
                   name: "search",
                   pattern: "search",
                   defaults: new { controller = "Shop", Action = "Search" }
                   );

                endpoints.MapControllerRoute(
                    name : "products" , 
                    pattern: "products/{category?}",
                    defaults: new {controller = "Shop" , Action = "List"}
                    );

                endpoints.MapControllerRoute(
                   name: "productdetails",
                   pattern: "{url}",
                   defaults: new { controller = "Shop", Action = "Details" }
                   );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                
            });
            SeedIdentity.Seed(userManager, roleManager, configuration).Wait();
        }
    }
}
