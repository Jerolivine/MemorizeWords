using MemorizeWords.Application.UserGuessedWords.Interfaces;
using MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces;
using Quartz;

namespace MemorizeWords.Quartz
{
    [DisallowConcurrentExecution]
    public class UserGuessedWordsJob : BaseJob
    {
        public IUserGuessedWordsService _userGuessedWords { get; set; }

        public UserGuessedWordsJob(IUserGuessedWordsService userGuessedWords,
            IApplicationLogger logger) : base(logger)
        {
            _userGuessedWords = userGuessedWords;
        }

        protected override async Task ExecuteJob()
        {
            await _userGuessedWords.PublishUserGuessedWords();
        }

        protected override string GetJobName()
        {
            return this.GetType().Name;
        }
    }
}
