using MemorizeWords.Application.Word.Interfaces;
using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Application.Interfaces;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using MemorizeWords.Presentation.Models.Request;
using MemorizeWords.Presentation.Models.Response;

namespace MemorizeWords.Application.Word.Services
{
    public class WordService : IWordService, IBusinessService
    {
        private IWordRepository _wordRepository { get; set; }

        public WordService(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        public async Task<WordEntity> AddWordAsync(WordAddRequest wordAddRequest)
        {
            var entity = await _wordRepository.AddWordAsync(wordAddRequest);
            return entity;
        }

        public async Task UpdateIsLearnedAsync(WordUpdateIsLearnedRequest wordUpdateIsLearnedRequest)
        {
            await _wordRepository.UpdateIsLearnedAsync(wordUpdateIsLearnedRequest);
        }

        public async Task<List<WordResponse>> UnLearnedWordsAsync()
        {
            return await _wordRepository.UnLearnedWordsAsync();
        }

        public async Task<List<WordResponse>> LearnedWordsAsync()
        {
            return await _wordRepository.LearnedWordsAsync();
        }

        public async Task<List<QuestionWordResponse>> GetQuestionWordsAsync()
        {
            return await _wordRepository.GetQuestionWordsAsync();
        }

        public async Task DeleteAsync(List<int> ids)
        {
            await _wordRepository.DeleteAsync(ids);
        }

    }
}
