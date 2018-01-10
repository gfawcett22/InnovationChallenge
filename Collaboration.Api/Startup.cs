using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR;
using Collaboration.Api.Hubs;
using RabbitMQ.Client;
using Collaboration.Data.Repositories;
using Collaboration.Core.Data;
using Collaboration.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Collaboration.Data.Seed;

namespace Collaboration.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc();

            var dbHostName = Environment.GetEnvironmentVariable("SQLSERVER_HOST") ?? "localhost";
            Console.WriteLine($"SQL Server Host: {dbHostName}");
            var dbPassword = Environment.GetEnvironmentVariable("SQLSERVER_SA_PASSWORD") ?? "Password123";
            Console.WriteLine($"SQL Server Host: {dbPassword}");
            var connString = $"Data Source={dbHostName};Initial Catalog=Collaboration;User ID=sa;Password={dbPassword};";
            services.AddDbContext<ThreadContext>(options => options.UseSqlServer(connString));
            
            services.AddTransient<ICollaborationService, CollaborationService>();
            services.AddTransient<IThreadRepository, ThreadRepository>();
            services.AddTransient<IPostRepository, PostRepository>();

            services.AddSingleton<IRabbitMQConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME") ?? "rabbit",
                    UserName = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "guest",
                    Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest",
                    Uri = new Uri("amqp://guest@localhost:5672")
                };
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

                return new RabbitMQEventBus(rabbitMQPersistentConnection, eventBusSubcriptionsManager, iLifetimeScope, "collaboration");
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            // Add Event Handlers Here
            services.AddTransient<ThreadUpdateIntegrationEventHandler>();
            services.AddTransient<PostUpdateIntegrationEventHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider sv)
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
            using (var db = sv.GetService<ThreadContext>())
            {
                ThreadSeeder.Seed(db);
            }
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
