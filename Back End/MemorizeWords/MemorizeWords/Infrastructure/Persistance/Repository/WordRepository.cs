using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Extensions;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using MemorizeWords.Infrastructure.Persistence.EfCore.Context;
using MemorizeWords.Infrastructure.Persistence.EfCore.Repository;
using MemorizeWords.Infrastructure.Persistence.Interfaces;
using MemorizeWords.Infrastructure.Persistence.Repository.Interfaces;
using MemorizeWords.Infrastructure.Transversal.Exception.Exceptions;
using MemorizeWords.Presentation.Models.Dto;
using MemorizeWords.Presentation.Models.Request;
using MemorizeWords.Presentation.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MemorizeWords.Infrastructure.Persistence.Repository
{
    public class WordRepository : EFCoreRepository<WordEntity, int>, IWordRepository, IBusinessRepository
    {
        private readonly IConfiguration _configuration;
        public IWordCommonRepository _wordCommonRepository { get; set; }

        public WordRepository(EFCoreDbContext dbContext,
            IConfiguration configuration,
            IWordCommonRepository wordCommonRepository) : base(dbContext)
        {
            _configuration = configuration;
            _wordCommonRepository = wordCommonRepository;
        }

        public async Task<WordEntity> AddWordAsync(WordAddRequest wordAddRequest)
        {
            ValidationAddUpdateWord(wordAddRequest);

            var wordEntity = await GetAsync(x => x.Word.Equals(wordAddRequest.Word.ToUpper()));
            if (wordEntity != null)
            {
                wordEntity.Meaning = wordAddRequest.Meaning.TrimEnd();
                wordEntity.WritingInLanguage = wordEntity.WritingInLanguage.TrimEnd();
                wordEntity.AskWordAgain = true;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                wordEntity = new WordEntity() { Word = wordAddRequest.Word.TrimEnd(), Meaning = wordAddRequest.Meaning.TrimEnd(), WritingInLanguage = wordAddRequest.WritingInLanguage.TrimEnd() };
                await AddAsnyc(wordEntity);
            }

            return wordEntity;
        }

        public async Task UpdateIsLearnedAsync(WordUpdateIsLearnedRequest wordUpdateIsLearnedRequest)
        {
            ValidationupdateIsLearned(wordUpdateIsLearnedRequest);
            await SetWordIsLearnedInformation(wordUpdateIsLearnedRequest);

        }

        private async Task SetWordIsLearnedInformation(WordUpdateIsLearnedRequest wordUpdateIsLearnedRequest)
        {
            if (wordUpdateIsLearnedRequest.IsLearned)
            {
                await Queryable().Where(x => wordUpdateIsLearnedRequest.Ids.Contains(x.Id))
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(n => n.IsLearned, n => wordUpdateIsLearnedRequest.IsLearned)
                     .SetProperty(n => n.LearnedDate, n => DateTime.Now.Date)
                );
            }
            else
            {
                await Queryable().Where(x => wordUpdateIsLearnedRequest.Ids.Contains(x.Id))
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(n => n.IsLearned, n => wordUpdateIsLearnedRequest.IsLearned)
                );
            }
        }

        public async Task<List<WordResponse>> UnLearnedWordsAsync()
            => await GetWordDetail(x => !x.IsLearned);

        private async Task<List<WordResponse>> GetWordDetail(Expression<Func<WordEntity, bool>> expression)
        {
            int sequentTrueAnswerCount = _configuration.GetSequentTrueAnswerCount();
            var words = await Queryable().Where(expression)
                        .Select(x => new
                        {
                            WordId = x.Id,
                            x.Meaning,
                            x.Word,
                            x.WritingInLanguage,
                            x.IsLearned,
                            x.AskWordAgain,
                            WordAnswers = x.WordAnswers.OrderByDescending(x => x.AnswerDate).Take(sequentTrueAnswerCount).Select(x => new WordAnswerDto()
                            {
                                Answer = x.Answer,
                                Id = x.Id,

                            }).ToList()
                        })
                        .Select(p => new WordResponse()
                        {
                            WordId = p.WordId,
                            Meaning = p.Meaning,
                            Word = p.Word,
                            WritingInLanguage = p.WritingInLanguage,
                            IsLearned = p.IsLearned,
                            WordAnswers = p.WordAnswers,
                            AskWordAgain = p.AskWordAgain
                        })
                        .ToListAsync();

            foreach (var word in words)
            {
                int trueAnswerCount = _wordCommonRepository.GetTrueAnswerCount(word);
                word.Percentage = ((double)trueAnswerCount / sequentTrueAnswerCount * 100).ToString();
            }

            return words;
        }

        public async Task<List<WordResponse>> LearnedWordsAsync()
        {

            var learnedWords = await Queryable().Where(x => x.IsLearned)
                        .Select(p => new WordResponse()
                        {
                            WordId = p.Id,
                            Meaning = p.Meaning,
                            Word = p.Word,
                            WritingInLanguage = p.WritingInLanguage,
                            AskWordAgain = p.AskWordAgain
                        })
                        .ToListAsync();

            return learnedWords;

        }

        private static void ValidationAddUpdateWord(WordAddRequest wordAddRequest)
        {
            NotImplementedBusinessException.ThrowIfNull(wordAddRequest, "Request Cannot Be Empty");
            NotImplementedBusinessException.ThrowIfNull(wordAddRequest?.Word, "Word Cannot Be Empty");
            NotImplementedBusinessException.ThrowIfNull(wordAddRequest?.Meaning, "Meaning Cannot Be Empty");
        }

        public async Task<List<QuestionWordResponse>> GetQuestionWordsAsync()
        {
            var randomWords = await Queryable().Where(x => !x.IsLearned)
                                   .OrderBy(x => Guid.NewGuid())
                                   .Take(20)
                                   .Select(x => new QuestionWordResponse()
                                   {
                                       Word = x.Word,
                                       Id = x.Id,
                                       WritingInLanguage = x.WritingInLanguage,
                                       Meaning = x.Meaning
                                   }).ToListAsync();

            return randomWords;
        }

        public async Task<List<int>> SetLearnedWordsSinceOneWeekAsUnlearnedAsync()
        {
            List<int> learnedWordsSinceOneWeekIds = await Queryable().Where(x => x.IsLearned && 
                                                                            x.LearnedDate <= DateTime.Now.AddDays(-7).Date &&
                                                                            x.AskWordAgain == true).Select(x => x.Id).ToListAsync();

            if (learnedWordsSinceOneWeekIds is null || learnedWordsSinceOneWeekIds.Count == 0)
            {
                return null;
            }

            await Queryable().Where(x => learnedWordsSinceOneWeekIds.Contains(x.Id))
                  .ExecuteUpdateAsync(s =>
                      s.SetProperty(n => n.IsLearned, n => false)
                       .SetProperty(n => n.LearnedDate, n => null));

            return learnedWordsSinceOneWeekIds;
        }

        public async Task DeleteAsync(List<int> ids)
        {
            await Queryable().Where(x => ids.Contains(x.Id))
                .ExecuteDeleteAsync();
        }

        public async Task DontAskThisWord(List<int> ids)
        {
            await Queryable().Where(x => ids.Contains(x.Id))
               .ExecuteUpdateAsync(s =>
                   s.SetProperty(n => n.AskWordAgain, n => false));
        }

        private static void ValidationupdateIsLearned(WordUpdateIsLearnedRequest wordUpdateIsLearnedRequest)
        {
            NotImplementedBusinessException.ThrowIfNull(wordUpdateIsLearnedRequest, "WordUpdateIsLearnedRequest Cannot Be Empty");
            NotImplementedBusinessException.ThrowIfNull(wordUpdateIsLearnedRequest.Ids, "Ids Cannot Be Empty");
        }

        public async Task<List<WordResponse>> GetWordAnswersHub(List<int> wordIds)
        {
            if(wordIds?.Count == 0)
            {
                return null;
            }

            return await GetWordDetail(x => wordIds.Contains(x.Id));
        }

    }
}
