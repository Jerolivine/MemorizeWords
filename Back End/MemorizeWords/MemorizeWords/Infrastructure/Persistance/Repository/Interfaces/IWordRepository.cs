using MemorizeWords.Api.Models.Request;
using MemorizeWords.Api.Models.Response;
using MemorizeWords.Entity;

namespace MemorizeWords.Infrastructure.Persistance.Repository.Interfaces
{
    public interface IWordRepository
    {
        Task<WordEntity> AddWordAsync(WordAddRequest wordAddRequest);
        Task UpdateIsLearnedAsync(WordUpdateIsLearnedRequest wordUpdateIsLearnedRequest);
        Task<List<WordResponse>> UnLearnedWordsAsync();
        Task<List<WordResponse>> LearnedWordsAsync();
        Task<List<QuestionWordResponse>> GetQuestionWordsAsync();
    }
}
