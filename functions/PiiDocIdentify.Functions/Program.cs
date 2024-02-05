using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiiDocIdentify.Functions;
using PiiDocIdentify.Functions.Extensions;
using Serilog;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((ctx, services) =>
    {
        services
            .RegisterOption<DocumentAnalysisClientSettings>(DocumentAnalysisClientSettings.ConfigurationSection)
            .ConfigureAzureClients(ctx.Configuration);
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .UseSerilog((ctx, sp, loggerCfg) => loggerCfg.ConfigureSerilog(sp, ctx.HostingEnvironment))
    .Build();

host.Run();