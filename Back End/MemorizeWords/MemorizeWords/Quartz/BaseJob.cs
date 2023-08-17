using MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces;
using Quartz;

namespace MemorizeWords.Quartz
{
    public abstract class BaseJob(IApplicationLogger _logger) : IJob
    {
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
