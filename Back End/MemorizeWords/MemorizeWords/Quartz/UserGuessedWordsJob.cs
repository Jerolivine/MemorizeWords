using MemorizeWords.Application.Word.Interfaces;
using Quartz;

namespace MemorizeWords.Quartz
{
    [DisallowConcurrentExecution]
    public class UserGuessedWordsJob : BaseJob
    {
        public IUserGuessedWords _userGuessedWords { get; set; }

        public UserGuessedWordsJob(IUserGuessedWords userGuessedWords)
        {
            _userGuessedWords = userGuessedWords;
        }

        protected override async Task ExecuteJob()
        {
            await _userGuessedWords.PublishUserGuessedWords();
        }
    }
}
