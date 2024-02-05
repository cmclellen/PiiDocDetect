using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiiDocIdentify.Functions;
using PiiDocIdentify.Functions.Extensions;

TokenCredential CreateTokenCredential()
{
    return new ChainedTokenCredential(
#if DEBUG
        new AzureCliCredential(),
#endif
        new ManagedIdentityCredential());
}

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((ctx, services) =>
    {
        var configuration = ctx.Configuration;
        services.RegisterOption<DocumentAnalysisClientSettings>(DocumentAnalysisClientSettings.ConfigurationSection);
        services.AddAzureClients(svc =>
        {
            svc.UseCredential(CreateTokenCredential());
            configuration.GetValue<string>("AzureWebJobsStorage");

            var documentAnalysisClient = configuration.GetSection("DocumentAnalysisClient");

            var documentAnalysisClientSettings = documentAnalysisClient.Get<DocumentAnalysisClientSettings>();
            svc.AddDocumentAnalysisClient(new Uri(documentAnalysisClientSettings.Endpoint));
        });
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();