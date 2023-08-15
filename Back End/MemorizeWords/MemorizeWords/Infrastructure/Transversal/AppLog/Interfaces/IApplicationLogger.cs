namespace MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces
{
    public interface IApplicationLogger
    {
        void LogInformation(string message);
        void LogVerbose(string message);
        void LogError(string message);
        void LogException(System.Exception exception, string? prefix = null);
    }
}
