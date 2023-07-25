using MemorizeWords.Application.UserGuessedWords.Interfaces;
using Quartz;

namespace MemorizeWords.Quartz
{
    [DisallowConcurrentExecution]
    public class UserGuessedWordsJob : BaseJob
    {
        public IUserGuessedWordsService _userGuessedWords { get; set; }

        public UserGuessedWordsJob(IUserGuessedWordsService userGuessedWords)
        {
            _userGuessedWords = userGuessedWords;
        }

        protected override async Task ExecuteJob()
        {
            await _userGuessedWords.PublishUserGuessedWords();
        }
    }
}
