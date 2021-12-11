using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using UserOrchestration.Consumers.EntityFrameworkCore;
using UserOrchestration.Core;
using UserOrchestration.Data.EntityFrameworkCore;
using UserOrchestration.EntityFrameworkCore;
using UserOrchestration.Utilities.Core;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Creating the Application Builder");
    var builder = WebApplication.CreateBuilder();

    builder.Host.ConfigureHostConfiguration(configure => configure.AddUserSecrets<Program>());

    Log.Information("Using Serilog");
    builder.Host.UseSerilog((context, configuration) => {
        configuration
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341");
    });

    Log.Information("Adding Services to the DI Container");
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(AutoMapper_MarkedAssemblies());

    Log.Information("Adding MassTransit to the DI Container");
    builder.Services.AddMassTransit(massTransit => {
        massTransit.AddRequestClient<CreateUser>();
        massTransit.AddRequestClient<GetUser>();

        massTransit.AddConsumersFromEntityFrameworkCore();
        massTransit.UsingRabbitMq((context, rabbitMq) => {
            rabbitMq.Host("rabbitmq://localhost:5672");
            rabbitMq.ConfigureEndpoints(context);
        });
    });

    builder.Services.AddMassTransitHostedService();

    Log.Information("Added UserOrchestration to the DI Container");
    builder.Services.AddUserOrchestration(userOrchestration => {
        userOrchestration
            .OnUtilities(utilities => utilities.Defaults())
            .AddEntityFrameworkCore(entityFrameworkCore => {
                entityFrameworkCore.AddUserDbContextFactory(options => options.UseNpgsql(builder.Configuration.GetConnectionString("UserOrchestration:Postgres:Development")));
            });
    });

    Log.Information("Configuring Json");
    builder.Services.Configure<JsonOptions>(options => {
        options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull;
    });

    Log.Information("Building the Application");
    var app = builder.Build();

    // app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        Log.Information("Using Swagger");
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    Log.Information("Mapping Endpoints");
    app.MapUserEndpoints();

    Log.Information("Running the Application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shutdown complete");
    Log.CloseAndFlush();
}

static Assembly[] AutoMapper_MarkedAssemblies() => new Assembly[]
{
    UserOrchestration.EntityFrameworkCore.AssemblyMarker.Assembly,
    UserOrchestration.WebApi.AssemblyMarker.Assembly
};