using MemorizeWords.Api.Models.Request;
using MemorizeWords.Api.Models.Response;

namespace MemorizeWords.Application.Word.Interfaces
{
    public interface IWordAnswerService
    {
        Task<AnswerResponse> AnswerAsync(WordAnswerRequest wordAnswerRequest);
        Task DeleteAllAnswersAsync(List<int> wordIds);
    }
}
