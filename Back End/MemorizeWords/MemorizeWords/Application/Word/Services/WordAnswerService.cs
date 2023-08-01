using MemorizeWords.Application.Word.Interfaces;
using MemorizeWords.Infrastructure.Application.Interfaces;
using MemorizeWords.Infrastructure.Persistence.Repository.Interfaces;
using MemorizeWords.Presentation.Models.Request;
using MemorizeWords.Presentation.Models.Response;

namespace MemorizeWords.Application.Word.Services
{
    public class WordAnswerService : IWordAnswerService, IBusinessService
    {
        private IWordAnswerRepository _wordAnswerRepository { get; set; }
        private IWordRepository _wordRepository { get; set; }
        public WordAnswerService(IWordAnswerRepository wordAnswerRepository,
            IWordRepository wordRepository)
        {
            _wordAnswerRepository = wordAnswerRepository;
            _wordRepository = wordRepository;   
        }

        public async Task<AnswerResponse> AnswerAsync(WordAnswerRequest wordAnswerRequest)
        {
            var answers = await _wordAnswerRepository.AnswerAsync(wordAnswerRequest);

            if (answers.IsAnswerTrue)
            {
                await CheckWordIsLearnedState(wordAnswerRequest);
            }

            return answers;
        }

        private async Task CheckWordIsLearnedState(WordAnswerRequest wordAnswerRequest)
        {
            var isAllAnswersTrue = await _wordAnswerRepository.IsAllAnswersTrue(wordAnswerRequest.WordId);
            if (isAllAnswersTrue)
            {
                await _wordRepository.UpdateIsLearnedAsync(new WordUpdateIsLearnedRequest()
                {
                    IsLearned = true,
                    Ids = new List<int> { wordAnswerRequest.WordId }
                });
            }
        }

        public async Task DeleteAllAnswersAsync(List<int> wordIds)
        {
            await _wordAnswerRepository.DeleteAllAnswersAsync(wordIds);
        }
    }
}
