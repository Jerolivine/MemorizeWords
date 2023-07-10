using MemorizeWords.Api.Models.Request;
using MemorizeWords.Api.Models.Response;
using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Persistance.Context.Repository;
using MemorizeWords.Infrastructure.Persistance.FCore.Context;
using MemorizeWords.Infrastructure.Persistance.Interfaces;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using MemorizeWords.Infrastructure.Transversal.Exception.Exceptions;
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

            //return Results.Ok(new AnswerResponse()
            //{
            //    IsAnswerTrue = isAnswerTrue,
            //    Meaning = wordEntity.Meaning
            //});

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
            wordEntity = _dbContext.Word.FirstOrDefault(x => x.Id == wordAnswerRequest.WordId);

            bool answer = wordEntity.Meaning.ToUpperInvariant().Equals(wordAnswerRequest.GivenAnswerMeaning.ToUpperInvariant().ToUpper(), StringComparison.OrdinalIgnoreCase);
            return answer;
        }
        private async Task<bool> IsAllAnswersTrue(int wordId)
        {
            int sequentTrueAnswerCount = GetSequentTrueAnswerCount();
            var answers = await Queryable().Where(x => x.WordId == wordId).OrderByDescending(x => x.AnswerDate).Take(sequentTrueAnswerCount).ToListAsync();
            if (answers?.Count != sequentTrueAnswerCount)
            {
                return false;
            }

            if (answers.Any(x => !x.Answer))
            {
                return true;
            }

            return false;


            // TODO- move to service side 
            //var wordEntityGivenAnswer = await Queryable().FirstOrDefaultAsync(x => x.Id == wordId);
            //if (wordEntityGivenAnswer == null)
            //{
            //    throw new KeyNotFoundBusinessException($"WordId: {wordId} not found");
            //}
            //wordEntityGivenAnswer.IsLearned = true;

            //await memorizeWordsDbContext.SaveChangesAsync();
        }
        private int GetSequentTrueAnswerCount()
        {
            int sequentTrueAnswerCount;
            int.TryParse(_configuration["SequentTrueAnswerCount"], out sequentTrueAnswerCount);

            return sequentTrueAnswerCount;
        }


    }
}
