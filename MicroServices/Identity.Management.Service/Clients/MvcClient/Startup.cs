using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace MvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment) {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }
        IConfiguration Configuration { get; }
        IHostingEnvironment HostingEnvironment { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var Issuer = Configuration.GetSection("AuthenticationServer")["Issuer"];
            var clientId = Configuration.GetSection("AuthenticationServer").GetSection("Client")["Id"];
            var clientSecret = Configuration.GetSection("AuthenticationServer").GetSection("Client")["Secret"];

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = Issuer;
                    options.RequireHttpsMetadata = false;

                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("roles");
                    options.Scope.Add("api1");
                    options.Scope.Add("Essence.Communication.Api");
                    //==================
                    // Fix for getting roles claims correctly :
                    options.ClaimActions.MapJsonKey("role", "role", "role");
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";
                    //===============

                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}