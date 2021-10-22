using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMWebAPI.Data;
using IMWebAPI.Helpers;
using IMWebAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

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

            services.AddIdentity<ApplicationUser, IdentityRole>()
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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.IncludeErrorDetails = true;
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "IMWEBAPI",//jwtBearerTokenSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = "IMWEBAPI",//jwtBearerTokenSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("IMWEBAPI_SECRETKEY")),//jwtBearerTokenSettings.Key)),
                        ValidateLifetime = true,
                        RequireAudience = false,
                        RequireExpirationTime = true,
                        RequireSignedTokens = true,
                        ClockSkew = TimeSpan.Zero // USED FOR TESTING
                    };
                    //options.Events = new JwtBearerEvents()
                    //{
                    //    OnChallenge = context =>
                    //    {
                    //        context.HandleResponse();
                    //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //        context.Response.ContentType = "application/json";

                    //        // Ensure we always have an error and error description.
                    //        if (string.IsNullOrEmpty(context.Error))
                    //            context.Error = "invalid_token";
                    //        if (string.IsNullOrEmpty(context.ErrorDescription))
                    //            context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                    //        // Add some extra context for expired tokens.
                    //        if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                    //        {
                    //            var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                    //            context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                    //            context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
                    //        }

                    //        return context.Response.WriteAsync(JsonSerializer.Serialize(new
                    //        {
                    //            error = context.Error,
                    //            error_description = context.ErrorDescription
                    //        }));
                    //    }

                    //};
                });
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
