using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IMWebAPI.Data;
using IMWebAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using IMWebAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IMWebAPI
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
            services.AddCors();
            services.AddControllers();

            services.AddDbContext<IM_API_Context>(options =>
                    options.UseMySQL(Configuration.GetConnectionString("IM_API_Context")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<IM_API_Context>();

            // custom services for dependency injection
            services.AddScoped<IEmailer, Emailer>();
            services.AddScoped<QueryRunner>();


            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 2;
                options.Password.RequiredUniqueChars = 0;
            });


            // configure settings for cookie authentication (Maybe switch to jwt in the future)

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "IMWEBAPI",//jwtBearerTokenSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = "IMWEBAPI",//jwtBearerTokenSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("IMWEBAPI_SECRETKEY")),//jwtBearerTokenSettings.Key)),
                        ValidateLifetime = false,
                        RequireAudience = true,
                        RequireExpirationTime = true
                    };
                });

                //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                //    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                //    {
                //        options.Cookie.Name = "IM_Auth_Cookie";
                //        options.SlidingExpiration = true;
                //        options.ExpireTimeSpan = new TimeSpan(0, 1, 0);
                //        options.Events = new CookieAuthenticationEvents
                //        {
                //            OnRedirectToLogin = redirectContext =>
                //            {
                //                redirectContext.HttpContext.Response.StatusCode = 401;
                //                return Task.CompletedTask;
                //            }
                //        };
                //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder
            .SetIsOriginAllowed(origin => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            //.WithOrigins("http://localhost:8080", "http://im.godandanime.tv")
            .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
