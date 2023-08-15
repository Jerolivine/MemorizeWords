using MemorizeWords.Infrastructure.Persistence.Repository.Interfaces;
using MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces;
using Quartz;

namespace MemorizeWords.Quartz
{
    [DisallowConcurrentExecution]
    public class SetLearnedWordToUnlearnedInOneWeekJob : BaseJob
    {
        public IWordRepository _wordRepository { get; set; }
        public IWordAnswerRepository _wordAnswerRepository { get; set; }
        public SetLearnedWordToUnlearnedInOneWeekJob(IWordRepository wordRepository,
            IWordAnswerRepository wordAnswerRepository,
            IApplicationLogger logger) :base(logger)
        {
            _wordRepository = wordRepository;
            _wordAnswerRepository = wordAnswerRepository;
        }

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
