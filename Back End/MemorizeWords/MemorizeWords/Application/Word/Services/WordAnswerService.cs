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
                await CheckWordIsLearnedState(wordAnswerRequest.WordId);
            }

            return answers;
        }

        public async Task<AnswerResponse> AnswerWordAsync(WordAnswerWordRequest wordAnswerWordRequest)
        {
            var answers = await _wordAnswerRepository.AnswerWordAsync(wordAnswerWordRequest);

            if (answers.IsAnswerTrue)
            {
                await CheckWordIsLearnedState(wordAnswerWordRequest.WordId);
            }

            return answers;
        }

        private async Task CheckWordIsLearnedState(int wordId)
        {
            var isAllAnswersTrue = await _wordAnswerRepository.IsAllAnswersTrue(wordId);
            if (isAllAnswersTrue)
            {
                await _wordRepository.UpdateIsLearnedAsync(new WordUpdateIsLearnedRequest()
                {
                    IsLearned = true,
                    Ids = new List<int> { wordId }
                });
            }
        }

        public async Task DeleteAllAnswersAsync(List<int> wordIds)
        {
            await _wordAnswerRepository.DeleteAllAnswersAsync(wordIds);
        }
    }
}
