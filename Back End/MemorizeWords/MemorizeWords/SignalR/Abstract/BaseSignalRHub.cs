using MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace MemorizeWords.SignalR.Abstract
{
    public abstract class BaseSignalRHub<T> : Hub<T> where T : class
    {
        public readonly IApplicationLogger _applicationLogger;
        public BaseSignalRHub(IApplicationLogger applicationLogger)  : base()
        {
            _applicationLogger = applicationLogger;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            LogOnDisconnectedException(exception);

            await base.OnDisconnectedAsync(exception);
        }

        private void LogOnDisconnectedException(Exception exception)
        {
            if (exception is not null)
            {
                var hubName = this.GetHubName();

                _applicationLogger.LogException(exception, "SignalR-error : " + hubName);
            }
        }

        protected abstract string GetHubName();

    }
}
