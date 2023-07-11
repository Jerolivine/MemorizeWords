using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using Quartz;

namespace MemorizeWords.Quartz
{
    [DisallowConcurrentExecution]
    public class SetLearnedWordToUnlearnedInOneWeekJob : IJob
    {
        public IWordRepository _wordRepository { get; set; }
        public IWordAnswerRepository _wordAnswerRepository { get; set; }
        public SetLearnedWordToUnlearnedInOneWeekJob(IWordRepository wordRepository,
            IWordAnswerRepository wordAnswerRepository)
        {
            _wordRepository = wordRepository;
            _wordAnswerRepository = wordAnswerRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var wordIds = await _wordRepository.SetLearnedWordsSinceOneWeekAsUnlearnedAsync();

            if (wordIds == null || wordIds.Count == 0)
            {
                return;
            }

            await _wordAnswerRepository.LeaveEnoughTrueAnswerToMemorize(wordIds);
        }

        //public async Task Execute(IJobExecutionContext context)
        //{
        //    Console.WriteLine("");
        //}
    }
}
