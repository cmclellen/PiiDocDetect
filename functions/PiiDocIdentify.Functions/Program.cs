using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiiDocIdentify.Functions;
using PiiDocIdentify.Functions.Extensions;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((ctx, services) =>
    {
        var configuration = ctx.Configuration;
        services
            .RegisterOption<DocumentAnalysisClientSettings>(DocumentAnalysisClientSettings.ConfigurationSection)
            .ConfigureSerilog(ctx.HostingEnvironment)
            .ConfigureAzureClients(ctx.Configuration);
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();