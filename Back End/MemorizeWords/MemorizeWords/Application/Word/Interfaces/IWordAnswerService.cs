using MemorizeWords.Presentation.Models.Request;
using MemorizeWords.Presentation.Models.Response;

namespace MemorizeWords.Application.Word.Interfaces
{
    public interface IWordAnswerService
    {
        Task<AnswerResponse> AnswerAsync(WordAnswerRequest wordAnswerRequest);
        Task<AnswerResponse> AnswerWordAsync(WordAnswerWordRequest wordAnswerWordRequest);
        Task DeleteAllAnswersAsync(List<int> wordIds);
    }
}
