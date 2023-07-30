using MemorizeWords.Entity;
using MemorizeWords.Presentation.Models.Request;
using MemorizeWords.Presentation.Models.Response;

namespace MemorizeWords.Infrastructure.Persistence.Repository.Interfaces
{
    public interface IWordRepository
    {
        Task<WordEntity> AddWordAsync(WordAddRequest wordAddRequest);
        Task UpdateIsLearnedAsync(WordUpdateIsLearnedRequest wordUpdateIsLearnedRequest);
        Task<List<WordResponse>> UnLearnedWordsAsync();
        Task<List<WordResponse>> LearnedWordsAsync();
        Task<List<QuestionWordResponse>> GetQuestionWordsAsync();
        Task<List<int>> SetLearnedWordsSinceOneWeekAsUnlearnedAsync();
        Task DeleteAsync(List<int> ids);
        Task<List<WordResponse>> GetWordAnswersHub(List<int> wordIds);
    }
}
