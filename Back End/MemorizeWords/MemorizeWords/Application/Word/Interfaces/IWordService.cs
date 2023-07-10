using MemorizeWords.Api.Models.Request;
using MemorizeWords.Api.Models.Response;
using MemorizeWords.Entity;

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
