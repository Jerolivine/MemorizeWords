using MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces;
using Serilog;

namespace MemorizeWords.Infrastructure.Transversal.AppLog.Serilog
{
    public class SeriLogLogger : IApplicationLogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public SeriLogLogger(Microsoft.Extensions.Logging.ILogger<SeriLogLogger> logger)
        {
            _logger = logger;
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
            //Log.Logger.Error(message);
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
            //Log.Logger.Information(message);
        }

        public void LogVerbose(string message)
        {
            _logger.LogDebug(message);
            //Log.Logger.Debug(message);
        }

        public void LogException(System.Exception exception, string? prefix = null)
        {
            if (exception is not null)
            {

                string errorLog = (prefix ?? string.Empty) + " " +
                         exception.Message + " " +
                         exception.InnerException + " " +
                         exception.StackTrace + " " +
                         " Exception Source:" + exception.Source;

                LogError(errorLog);
            }
        }

    }
}
