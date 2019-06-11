using System.Reflection;
using Identity.Management.Api.Extensions;
using Identity.Management.Api.Services;
using Identity.Management.DataAccess;
using Identity.Management.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.Management.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }
        private readonly ILogger _logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ConfigOptions>(Configuration);
            var sp = services.BuildServiceProvider();
            var configOptions = sp.GetService<IOptionsMonitor<ConfigOptions>>().CurrentValue;

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            
            var applicationIdentityConnectionString = configOptions.ConnectionStrings.ApplicationIdentityConnectionString;
            services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(applicationIdentityConnectionString,sql=>sql.MigrationsAssembly(configOptions.MigrationAssembly)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //var oprationsConnectionString = Configuration.GetConnectionString("IdentityServerOprationsConnectionString");
            var oprationsConnectionString = configOptions.ConnectionStrings.IdentityServerOprationsConnectionString;

            ///Todo:
            ///Get the ssl certificate installed locally with the name *.homestay.care
            ///Setup the containres to use a host *.homestay.care with the same certificate in dev environment
            ///Install the Certificate in AWS store and setup the hostname for identity server and main api
            ///For local tests, Api should not be run from container by the time above solution will fix the issue 
            
            var identityServerBuilder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                //options.IssuerUri = "https://localhost:44343";
                //options.PublicOrigin = "https://localhost:44343";

                //options.IssuerUri = "http://identitymanagementapi-1966185121.ap-southeast-2.elb.amazonaws.com";
                //options.PublicOrigin = "http://identitymanagementapi-1966185121.ap-southeast-2.elb.amazonaws.com";
            })

                //.AddDeveloperSigningCredential()
                //.AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddInMemoryApiResources(Config.GetApiResources())
                //.AddInMemoryClients(Config.GetClients())
                //.AddTestUsers(Config.GetUsers());

                .AddSigninCredentialFromConfig(Configuration, _logger)
                //.AddDeveloperSigningCredential()
                //.AddTestUsers(Config.GetUsers()).
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(oprationsConnectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(oprationsConnectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService>(); 

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();
        }
    }
}
