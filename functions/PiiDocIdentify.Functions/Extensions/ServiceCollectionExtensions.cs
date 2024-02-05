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
    }
}