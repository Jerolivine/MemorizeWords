using MemorizeWords.Infrastructure.Constants.Framework;
using MemorizeWords.Infrastructure.Extensions;
using MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces;
using MemorizeWords.Infrastructure.Transversal.AppLog.Serilog;
using Serilog;

namespace MemorizeWords.Infrastructure.Configuration
{
    public static class LogConfiguration
    {
        public static void ConfigureLog(this WebApplicationBuilder builder)
        {
            ConfigureSerilog(builder);
        }

        
        public static void ConfigureLog(this WebApplication app, ConfigurationManager configuration)
        {
            ConfigureSerilog(app, configuration);

        }

        private static void ConfigureSerilog(WebApplication app, ConfigurationManager configuration)
        {
            var useSeriLog = configuration.GetSettingsValue<bool>(AppSettingsConstants.USE_SERILOG);

            if (useSeriLog)
            {
                app.Logger.LogInformation("Serilog Configured");
            }
        }

        private static void ConfigureSerilog(WebApplicationBuilder builder)
        {
            var useSeriLog = builder.Configuration.GetSettingsValue<bool>(AppSettingsConstants.USE_SERILOG);

            if (useSeriLog)
            {

                // remove default logging providers
                builder.Logging.ClearProviders();

                // Serilog configuration        
                var logger = new LoggerConfiguration()
                             .WriteTo.File("Logs/logs-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                             .WriteTo.File("Logs/error-logs-.txt", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
                             .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose)
                             .CreateLogger();

                // Register Serilog
                builder.Logging.AddSerilog(logger);

                builder.Services.AddTransient<IApplicationLogger, SeriLogLogger>();    

            }
        }

    }
}
