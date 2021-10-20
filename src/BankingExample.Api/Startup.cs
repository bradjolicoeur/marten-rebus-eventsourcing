using BankingExample.Api.Helpers;
using BankingExample.Api.Middleware;
using BankingExample.Bus.BusHandlers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Rebus.Config;
using Rebus.ServiceProvider;

namespace BankingExample.Api
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
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BankingExample.Api", Version = "v1" });
            });

            services.AddMartenConfig(Configuration);

            services.AddMediatR(typeof(Startup).Assembly);
            services.AddAutoMapper(typeof(Startup));
            services.AddValidatorsFromAssemblyContaining<Startup>();

            // Register Rebus handlers 
            services.AutoRegisterHandlersFromAssemblyOf<PostTransactionHandler>();

            // Configure and register Rebus
            services.AddRebus(configure => configure
                .Transport(t => t.UseRabbitMq("amqp://rabbitmq:rabbitmq@localhost", "bankingexample.webapp"))
                //.Routing(r => r.TypeBased().MapAssemblyOf<Startup>("example.paymentsaga"))
                );

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankingExample.Api v1"));
            }

            app.ApplicationServices.UseRebus();
            //or optionally act on the bus
            //app.ApplicationServices.UseRebus(async bus => await bus.Subscribe<ICompletedMakePayment>());

           
            app.UseHealthChecks("/health");

            app.UseRouting();

            app.UseAuthorization();

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
