using Asp.Versioning;
using BankingExample.Api.Helpers;
using BankingExample.Api.Middleware;
using BankingExample.Bus.BusHandlers;
using BankingExample.Handlers;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Rebus.Config;
using System;
using Oakton;
using Oakton.Resources;
using Wolverine;
using Wolverine.Http;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BankingExample.Api", Version = "v1" });
    c.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);
});

builder.Services.AddMartenConfig(builder.Configuration)
    .IntegrateWithWolverine();

builder.Services.AddResourceSetupOnStartup();

// Wolverine usage is required for WolverineFx.Http
builder.Host.UseWolverine(opts =>
{
    // This middleware will apply to the HTTP
    // endpoints as well
    opts.Policies.AutoApplyTransactions();

    // Setting up the outbox on all locally handled
    // background tasks
    opts.Policies.UseDurableLocalQueues();
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddAutoMapper(typeof(AcceptTransactionHandler).Assembly, typeof(Program).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Register Rebus handlers 
builder.Services.AutoRegisterHandlersFromAssemblyOf<PostTransactionHandler>();

// Configure and register Rebus
var rabbitMqConnection = builder.Configuration["RABBITMQ"];
var inboundQueueName = builder.Configuration["INPUT_QUEUE"];
builder.Services.AddRebus(configure => configure
    .Transport(t => t.UseRabbitMq(rabbitMqConnection, inboundQueueName))
    //.Routing(r => r.TypeBased().MapAssemblyOf<Startup>("example.paymentsaga"))
    );

builder.Services.AddHealthChecks();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BankingExample.Api v1"));
}


app.UseHealthChecks("/health");

app.UseRouting();

app.UseAuthorization();

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapWolverineEndpoints();

return await app.RunOaktonCommands(args);
