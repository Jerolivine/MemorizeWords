using MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces;
using Serilog;

namespace MemorizeWords.Infrastructure.Transversal.AppLog.Serilog
{
    public class SeriLogLogger : IApplicationLogger
    {
        public void LogError(string message)
        {
            Log.Logger.Error(message);
        }

        public void LogInformation(string message)
        {
            Log.Logger.Information(message);
        }

        public void LogVerbose(string message)
        {
            Log.Logger.Debug(message);
        }
    }
}
