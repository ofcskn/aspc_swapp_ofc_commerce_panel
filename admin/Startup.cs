using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using admin.Hubs;
using admin.Mappings;
using admin.Models;
using AutoMapper;
using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Abstract;
using Service.Concrete.EntityFramework;
using Service.Utilities;

namespace admin
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllersWithViews(x => x.SuppressAsyncSuffixInActionNames = false)
                    .AddRazorRuntimeCompilation();

            services.AddTransient<IViewRenderService, ViewRenderService>();
            services.AddTransient<IMediaService, EfMediaService>();
            services.AddTransient<IUnitOfWork, EfUnitOfWork>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.LoginPath = "/home/login";
              });


            //Role Services
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Staff", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "admin", "user", "staff");
                });

                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "admin");
                });

                options.AddPolicy("Current", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "admin", "current");
                });
                
                options.AddPolicy("User", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "admin", "user" );
                });
                
               
            });


            if (Environment.IsDevelopment())
            {
                services.AddControllersWithViews(x => x.SuppressAsyncSuffixInActionNames = false)
                .AddRazorRuntimeCompilation();

                services.AddDbContext<SwappDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Development")));

            }
            else
            {
                services.AddControllersWithViews();
                services.AddDbContext<SwappDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Production")));
            }

            services.AddAutoMapper(typeof(Startup));
            services.AddRazorPages();
            services.AddControllers();
            services.AddSignalR();
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/500");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseHsts();
            }
            //Register GleamTech to the ASP.NET Core HTTP request pipeline.
            ////----------------------
            //app.UseGleamTech();
            ////--------------------
            //Set this property only if you have a valid license key, otherwise do not 
            //set it so FileUltimate runs in trial mode.  
            //FileUltimateConfiguration.Current.LicenseKey = "QQJDJLJP34...";
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/message/chatter");
                endpoints.MapHub<NotificationHub>("/notification/hub");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
