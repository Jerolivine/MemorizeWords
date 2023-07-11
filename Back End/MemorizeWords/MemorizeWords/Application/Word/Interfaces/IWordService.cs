using MemorizeWords.Entity;
using MemorizeWords.Presentation.Models.Request;
using MemorizeWords.Presentation.Models.Response;

namespace MemorizeWords.Application.Word.Interfaces
{
    public interface IWordService
    {
        Task<WordEntity> AddWordAsync(WordAddRequest wordAddRequest);
        Task UpdateIsLearnedAsync(WordUpdateIsLearnedRequest wordUpdateIsLearnedRequest);
        Task<List<WordResponse>> UnLearnedWordsAsync();
        Task<List<WordResponse>> LearnedWordsAsync();
        Task<List<QuestionWordResponse>> GetQuestionWordsAsync();
    }
}
