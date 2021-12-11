using System.Reflection;
using Serilog;
using Serilog.Events;
using UserOrchestration.Consumers.MailingService;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Creating the Host Builder");
    var builder = Host.CreateDefaultBuilder(args);

    Log.Information("Using Serilog");
    builder.ConfigureLogging(logging => {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();

        logging.AddSerilog(logger, true);
    });

    Log.Information("Adding services to the DI Container");
    builder.ConfigureServices(services => {
        services.AddAutoMapper(AutoMapper_MarkedAssemblies());
        services.AddMassTransit(massTransit => {
            massTransit.AddConsumersFromMailingService();
            massTransit.UsingRabbitMq((context, rabbitMq) => {
                rabbitMq.Host("rabbitmq://localhost:5672");
                rabbitMq.ConfigureEndpoints(context);
            });
        });

        services.AddMassTransitHostedService();
    });

    Log.Information("Building the Host");
    var host = builder.Build();

    Log.Information("Running the Host");
    await host.RunAsync();
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
    UserOrchestration.Core.AssemblyMarker.Assembly
};