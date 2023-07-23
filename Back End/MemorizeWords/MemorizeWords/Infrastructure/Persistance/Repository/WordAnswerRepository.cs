using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Constants;
using MemorizeWords.Infrastructure.Constants.AppSettings;
using MemorizeWords.Infrastructure.Extensions;
using MemorizeWords.Infrastructure.Persistance.Context.Repository;
using MemorizeWords.Infrastructure.Persistance.FCore.Context;
using MemorizeWords.Infrastructure.Persistance.Interfaces;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using MemorizeWords.Infrastructure.Transversal.Exception.Exceptions;
using MemorizeWords.Presentation.Models.Request;
using MemorizeWords.Presentation.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace MemorizeWords.Infrastructure.Persistance.Repository
{
    public class WordAnswerRepository : EFCoreRepository<WordAnswerEntity, int>, IWordAnswerRepository, IBusinessRepository
    {
        private readonly IConfiguration _configuration;
        public WordAnswerRepository(EFCoreDbContext dbContext, IConfiguration configuration) : base(dbContext)
        {
            _configuration = configuration;
        }
        public async Task<AnswerResponse> AnswerAsync(WordAnswerRequest wordAnswerRequest)
        {
            ValidationAnswer(wordAnswerRequest);

            WordEntity wordEntity;
            bool isAnswerTrue = GetGivenAnswer(wordAnswerRequest, out wordEntity);

            await AddAsnyc(new WordAnswerEntity()
            {
                WordId = wordAnswerRequest.WordId,
                Answer = isAnswerTrue,
                AnswerDate = DateTime.Now,
            });


            if (isAnswerTrue)
            {
                await IsAllAnswersTrue(wordAnswerRequest.WordId);
            }

            return new AnswerResponse()
            {
                IsAnswerTrue = isAnswerTrue,
                Meaning = wordEntity.Meaning
            };
        }

        public async Task DeleteAllAnswersAsync(List<int> wordIds)
        {
            await Queryable().Where(x => wordIds.Contains(x.WordId))
               .ExecuteDeleteAsync();
        }

        public async Task LeaveEnoughTrueAnswerToMemorize(List<int> wordIds)
        {
            int enoughAnswerToMemorize = _configuration.GetEnoughAnswerToMemorize();

            await Queryable().Where(x => wordIds.Contains(x.WordId))
               .ExecuteDeleteAsync();

            await AddEnoughTruAnswer(wordIds, enoughAnswerToMemorize);

            await _dbContext.SaveChangesAsync();

        }

        private async Task AddEnoughTruAnswer(List<int> wordIds, int enoughAnswerToMemorize)
        {
            foreach (var wordId in wordIds)
            {
                for (int i = 0; i < enoughAnswerToMemorize; i++)
                {
                    await _dbContext.WordAnswer.AddAsync(new WordAnswerEntity()
                    {
                        WordId = wordId,
                        Answer = true,
                        AnswerDate = DateTime.Now
                    });
                }
            }
        }

        private void ValidationAnswer(WordAnswerRequest wordAnswerRequest)
        {
            NotImplementedBusinessException.ThrowIfNull(wordAnswerRequest, "Request Cannot Be Empty");
            NotImplementedBusinessException.ThrowIfNull(wordAnswerRequest?.WordId, "WordId Cannot Be Empty");
            NotImplementedBusinessException.ThrowIfNull(wordAnswerRequest?.GivenAnswerMeaning, "GivenAnswerMeaning Cannot Be Empty");

            var wordEntity = _dbContext.Word.FirstOrDefault(x => x.Id == wordAnswerRequest.WordId);
            NotImplementedBusinessException.ThrowIfNull(wordEntity, $"Word Couldnt found by given Id, {wordAnswerRequest.WordId}");
        }
        private bool GetGivenAnswer(WordAnswerRequest wordAnswerRequest, out WordEntity wordEntity)
        {
            wordEntity = _dbContext.Word.FirstOrDefault(x => x.Id == wordAnswerRequest.WordId) ?? throw new KeyNotFoundBusinessException($"wordId: {wordAnswerRequest.WordId} couldn't found");
            
            string targetLanguage = GetCrossCheckWord(wordAnswerRequest.AnswerLanguageType, wordEntity);

            bool answer = targetLanguage.ToUpperInvariant().Equals(wordAnswerRequest.GivenAnswerMeaning.ToUpperInvariant().ToUpper(), StringComparison.OrdinalIgnoreCase);
            return answer;
        }

        private string GetCrossCheckWord(int answerLanguageType, WordEntity wordEntity)
        {
            return (LanguageType)answerLanguageType == LanguageType.LearningLanguage
                ? wordEntity.Word
                : wordEntity.Meaning;
        }
        
        private async Task<bool> IsAllAnswersTrue(int wordId)
        {
            int sequentTrueAnswerCount = _configuration.GetSequentTrueAnswerCount();
            var answers = await Queryable().Where(x => x.WordId == wordId).OrderByDescending(x => x.AnswerDate).Take(sequentTrueAnswerCount).ToListAsync();
            if (answers?.Count != sequentTrueAnswerCount)
            {
                return false;
            }

            if (answers.Exists(x => !x.Answer))
            {
                return true;
            }

            return false;

        }

    }
}
