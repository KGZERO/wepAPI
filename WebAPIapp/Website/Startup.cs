using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIapp.Data;
using WebAPIapp.Model;
using WebAPIapp.Services;
using Website.Services;

namespace Website
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
           
            services.AddHttpClient();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Login/";
                    options.AccessDeniedPath = "/User/Forbidden/";
                });
            services.AddDbContext<MyDbContext>(option => {
                option.UseSqlServer(Configuration.GetConnectionString("MyDb"));
            });
            services.AddControllersWithViews()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginModel>());
            services.Configure<AppSetting>(Configuration.GetSection("AppSettings"));
            services.AddTransient<IUserServices, UserServices>();
            services.AddScoped<IUserResponsitory, UserResponsitory>();
            IMvcBuilder builder = services.AddRazorPages();
            var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
#if DEBUG
            if (enviroment == Environments.Development)
            {
                builder.AddRazorRuntimeCompilation();
            }
#endif
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            //{
            //    opt.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
                    
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ValidateLifetime = false,
            //        RequireExpirationTime = false,

            //        // Allow to use seconds for expiration of token
            //        // Required only when token lifetime less than 5 minutes
            //        // THIS ONE
            //        ClockSkew = TimeSpan.Zero
            //    };
            //});
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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
