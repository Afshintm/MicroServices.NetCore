using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BuildingBlocks.EventBus.Interfaces;
using BuildingBlocks.EventBus.MessageQueue;
using BuildingBlocks.EventBus.MessageQueue.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Services.Utils;

namespace Core.Service.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add services to the collection.
            services.AddCors();
            services.AddMvc();


            var builder = AppContainerBuilder(services);
            //var builder = services.GetAppContainerBuilder();

            RegisterServiceBusMessageEvents(services);

            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        private static ContainerBuilder AppContainerBuilder(IServiceCollection services)
        {
            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register dependencies, populate the services from
            // the collection, and build the container. If you want
            // to dispose of the container at the end of the app,
            // be sure to keep a reference to it as a property or field.
            //
            // Note that Populate is basically a foreach to add things
            // into Autofac that are in the collection. If you register
            // things in Autofac BEFORE Populate then the stuff in the
            // ServiceCollection can override those things; if you register
            // AFTER Populate those registrations can override things
            // in the ServiceCollection. Mix and match as needed.
            builder.Populate(services);
            builder.RegisterType(typeof(EventBusMessageQueue)).As(typeof(IEventBus)).SingleInstance();
            builder.RegisterType(typeof(MQPersistantConnection)).As(typeof(IMQPersistentConnection)).InstancePerLifetimeScope();

            return builder;
        }

        // Configure is where you add middleware. This is called after
        // ConfigureServices. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(
          IApplicationBuilder app,
          ILoggerFactory loggerFactory,
          IApplicationLifetime appLifetime, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            //        if (env.IsDevelopment())
            //        {
            //            app.UseDeveloperExceptionPage();
            //        }
            //        else
            //        {
            //            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //            app.UseHsts();
            //        }

            //        app.UseHttpsRedirection();



            //loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            //app.UseCheckRequestMiddleware();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseMvc();
            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            // You can only do this if you have a direct reference to the container,
            // so it won't work with the above ConfigureContainer mechanism.
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
            //var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
        }

        private void RegisterServiceBusMessageEvents(IServiceCollection services)
        {
            var eventBus = services.AddSingleton<IEventBus, EventBusMessageQueue>(sp =>
            {
                var serviceBusPersisterConnection = sp.GetRequiredService<IMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                //var eventBus = sp.GetRequiredService<IEventBus>();
                //eventBus.Subscribe<EventObjectStructure, >();
                return new EventBusMessageQueue(serviceBusPersisterConnection, Configuration);
            });


        }
    }
}
