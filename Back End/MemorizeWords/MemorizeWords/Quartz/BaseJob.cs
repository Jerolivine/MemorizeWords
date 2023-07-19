using Quartz;

namespace MemorizeWords.Quartz
{
    public abstract class BaseJob : IJob
    {
        protected abstract Task ExecuteJob();

        async Task IJob.Execute(IJobExecutionContext context)
        {
            await ExecuteJob();
            //try
            //{
            //    ExecuteJob();
            //}
            //catch (Exception ex)
            //{
            //    // todo log
            //    throw ex;
            //}

            return;
        }
    }
}
