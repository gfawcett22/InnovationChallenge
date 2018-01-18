using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Collaboration.Api.IntegrationEvents.EventHandling;
using Collaboration.Api.IntegrationEvents.Events;
using Collaboration.Messaging.Models;
using Collaboration.Messaging.Models.Abstractions;
using Collaboration.Messaging.RabbitMQ;
using Collaboration.Messaging.RabbitMQ.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Collaboration.Api.Hubs;
using RabbitMQ.Client;
using Collaboration.Data.Repositories;
using Collaboration.Core.Data;
using Collaboration.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Collaboration.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc();

            services.AddDbContext<ThreadContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"],
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                     });

                // Changing default behavior when client evaluation occurs to throw. 
                // Default in EF Core would be to log a warning when client evaluation is performed.
                options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            });

            services.AddTransient<ICollaborationService, CollaborationService>();
            services.AddTransient<IThreadRepository, ThreadRepository>();
            services.AddTransient<IPostRepository, PostRepository>();

            services.AddSingleton<IRabbitMQConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"]
                };

                if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                {
                    factory.UserName = Configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                {
                    factory.Password = Configuration["EventBusPassword"];
                }
                return new RabbitMQConnection(factory);
            });

            RegisterEventBus(services);

            services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                });

            services.AddSignalR();

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQConnection>();
                var logger = sp.GetRequiredService<ILogger<RabbitMQEventBus>>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new RabbitMQEventBus(rabbitMQPersistentConnection, eventBusSubcriptionsManager, iLifetimeScope);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            // Add Event Handlers Here
            services.AddTransient<ThreadUpdateIntegrationEventHandler>();
            services.AddTransient<PostUpdateIntegrationEventHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider sv, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("CorsPolicy");

            app.UseMvc();

            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<CollaborationHub>("CollaborationHub");
            });

            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            // Subscribe to events and put the handler here
            eventBus.Subscribe<ThreadUpdateIntegrationEvent, ThreadUpdateIntegrationEventHandler>();
            eventBus.Subscribe<PostUpdateIntegrationEvent, PostUpdateIntegrationEventHandler>();
        }

    }
}
