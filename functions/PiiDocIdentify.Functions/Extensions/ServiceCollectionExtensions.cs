﻿using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PiiDocIdentify.Functions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterOption<TSetting>(this IServiceCollection services,
            string configurationSection) where TSetting : class
        {
            services.AddOptions<TSetting>()
                .BindConfiguration(configurationSection)
                .ValidateDataAnnotations();
            //.ValidateOnStart();
            return services;
        }

        public static IServiceCollection ConfigureAzureClients(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAzureClients(svc =>
            {
                svc.UseCredential(CreateTokenCredential());
                configuration.GetValue<string>("AzureWebJobsStorage");

                var documentAnalysisClient = configuration.GetSection("DocumentAnalysisClient");

                var documentAnalysisClientSettings = documentAnalysisClient.Get<DocumentAnalysisClientSettings>();
                svc.AddDocumentAnalysisClient(new Uri(documentAnalysisClientSettings.Endpoint));
            });
            return services;
        }

        private static TokenCredential CreateTokenCredential()
        {
            return new ChainedTokenCredential(
#if DEBUG
                new AzureCliCredential(),
#endif
                new ManagedIdentityCredential());
        }
    }
}