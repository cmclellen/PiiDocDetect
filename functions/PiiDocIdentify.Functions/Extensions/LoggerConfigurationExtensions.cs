using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace PiiDocIdentify.Functions.Extensions
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration ConfigureSerilog(this LoggerConfiguration loggerConfiguration,
            IServiceProvider sp, IHostEnvironment env)
        {
            loggerConfiguration = loggerConfiguration.MinimumLevel.Debug();
            if (env.IsProduction())
            {
                loggerConfiguration = loggerConfiguration.WriteTo.ApplicationInsights(
                    sp.GetRequiredService<TelemetryClient>(),
                    TelemetryConverter.Traces);
            }
            else
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Console(theme: AnsiConsoleTheme.Code);
            }

            return loggerConfiguration;
        }
    }
}