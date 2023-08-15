using MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces;
using Quartz;

namespace MemorizeWords.Quartz
{
    public abstract class BaseJob : IJob
    {
        public readonly IApplicationLogger _logger;

        public BaseJob(IApplicationLogger logger)
        {
            _logger = logger;
        }

        protected abstract Task ExecuteJob();

        protected abstract string GetJobName();

        async Task IJob.Execute(IJobExecutionContext context)
        {
            try
            {
                await ExecuteJob();
            }
            catch (Exception ex)
            {
                var jobName = this.GetJobName();
                _logger.LogException(ex, jobName);
            }

        }
    }
}
