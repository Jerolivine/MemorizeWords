using MemorizeWords.Api.Models.Request;
using MemorizeWords.Api.Models.Response;

namespace MemorizeWords.Infrastructure.Persistance.Repository.Interfaces
{
    public interface IWordAnswerRepository
    {
        Task<AnswerResponse> AnswerAsync(WordAnswerRequest wordAnswerRequest);
        Task DeleteAllAnswersAsync(List<int> wordIds);
    }
}
