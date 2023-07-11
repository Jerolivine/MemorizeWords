using MemorizeWords.Application.Word.Interfaces;
using MemorizeWords.Infrastructure.Application.Interfaces;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using MemorizeWords.Presentation.Models.Request;
using MemorizeWords.Presentation.Models.Response;

namespace MemorizeWords.Application.Word.Services
{
    public class WordAnswerService : IWordAnswerService, IBusinessService
    {
        private IWordAnswerRepository _wordAnswerRepository { get; set; }
        public WordAnswerService(IWordAnswerRepository wordAnswerRepository)
        {
            _wordAnswerRepository = wordAnswerRepository;
        }

        public async Task<AnswerResponse> AnswerAsync(WordAnswerRequest wordAnswerRequest)
        {
            var answers = await _wordAnswerRepository.AnswerAsync(wordAnswerRequest);
            return answers;
        }

        public async Task DeleteAllAnswersAsync(List<int> wordIds)
        {
            await _wordAnswerRepository.DeleteAllAnswersAsync(wordIds);
        }
    }
}
