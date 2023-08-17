using MemorizeWords.Infrastructure.Persistence.Repository.Interfaces;
using MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces;
using Quartz;

namespace MemorizeWords.Quartz
{
    [DisallowConcurrentExecution]
    public class SetLearnedWordToUnlearnedInOneWeekJob(IWordRepository _wordRepository,
                                                       IWordAnswerRepository _wordAnswerRepository,
                                                       IApplicationLogger logger) : BaseJob(logger)
    {
        protected override async Task ExecuteJob()
        {
            var wordIds = await _wordRepository.SetLearnedWordsSinceOneWeekAsUnlearnedAsync();

            if (wordIds is null || wordIds.Count == 0)
            {
                return;
            }

            await _wordAnswerRepository.LeaveEnoughTrueAnswerToMemorize(wordIds);
        }

        protected override string GetJobName()
        {
            return this.GetType().Name;
        }
    }
}
