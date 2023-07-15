using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Constants.AppSettings;
using MemorizeWords.Infrastructure.Extensions;
using MemorizeWords.Infrastructure.Persistance.Context.Repository;
using MemorizeWords.Infrastructure.Persistance.FCore.Context;
using MemorizeWords.Infrastructure.Persistance.Interfaces;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using MemorizeWords.Infrastructure.Transversal.Exception.Exceptions;
using MemorizeWords.Presentation.Models.Dto;
using MemorizeWords.Presentation.Models.Request;
using MemorizeWords.Presentation.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace MemorizeWords.Infrastructure.Persistance.Repository
{
    public class WordRepository : EFCoreRepository<WordEntity, int>, IWordRepository, IBusinessRepository
    {
        private readonly IConfiguration _configuration;

        public WordRepository(EFCoreDbContext dbContext,
            IConfiguration configuration) : base(dbContext)
        {
            _configuration = configuration;
        }

        public async Task<WordEntity> AddWordAsync(WordAddRequest wordAddRequest)
        {
            ValidationAddUpdateWord(wordAddRequest);

            var wordEntity = await GetAsync(x => x.Word.Equals(wordAddRequest.Word.ToUpper()));
            if (wordEntity != null)
            {
                wordEntity.Meaning = wordAddRequest.Meaning.TrimEnd();
                wordEntity.WritingInLanguage = wordEntity.WritingInLanguage.TrimEnd();
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
            ValidationupdateIsLearned(wordUpdateIsLearnedRequest.Ids);

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
        {

            int sequentTrueAnswerCount = _configuration.GetSequentTrueAnswerCount();
            var unlearnedWords = await Queryable().Where(x => !x.IsLearned)
                        .Select(x => new
                        {
                            WordId = x.Id,
                            Meaning = x.Meaning,
                            Word = x.Word,
                            WritingInLanguage = x.WritingInLanguage,
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
                            WordAnswers = p.WordAnswers,
                            WritingInLanguage = p.WritingInLanguage,
                            //Percentage = ((((double)p.WordAnswers.Where(x => x.Answer).Count() / sequentTrueAnswerCount)) * 100).ToString()
                        })
                        .ToListAsync();

            foreach (var unlearnedWord in unlearnedWords)
            {
                int trueAnswerCount = GetTrueAnswerCount(unlearnedWord);
                unlearnedWord.Percentage = (((double)trueAnswerCount / sequentTrueAnswerCount) * 100).ToString();
            }

            return unlearnedWords;
        }

        public async Task<List<WordResponse>> LearnedWordsAsync()
        {

            var learnedWords = await Queryable().Where(x => x.IsLearned)
                        .Select(p => new WordResponse()
                        {
                            WordId = p.Id,
                            Meaning = p.Meaning,
                            Word = p.Word,
                            WritingInLanguage = p.WritingInLanguage
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
            var learningLanguageWordCount = new Random().Next(0, 21);
            var userLanguageWordCount = 20 - learningLanguageWordCount;

            var baseQuery = Queryable().Where(x => !x.IsLearned).OrderBy(x => Guid.NewGuid());

            var learningLanguageWords = await baseQuery
                .Take(learningLanguageWordCount)
                .Select(x => new QuestionWordResponse()
                {
                    Word = x.Word,
                    Id = x.Id,
                    WritingInLanguage = x.WritingInLanguage,
                    Meaning = x.Meaning,
                    LanguageType = (int)LanguageType.UserLanguage
                }).ToListAsync();

            var userLanguageWords = await baseQuery
                .Take(userLanguageWordCount)
                .Select(x => new QuestionWordResponse()
                {
                    Word = x.Meaning,
                    Id = x.Id,
                    Meaning = x.Word,
                    LanguageType = (int)LanguageType.LearningLanguage
                }).ToListAsync();

            var randomWords = learningLanguageWords.Concat(userLanguageWords).ToList();

            return randomWords;
        }

        public async Task<List<int>> SetLearnedWordsSinceOneWeekAsUnlearnedAsync()
        {
            List<int> learnedWordsSinceOneWeekIds = await Queryable().Where(x => x.IsLearned && x.LearnedDate < DateTime.Now.AddDays(-7).Date).Select(x => x.Id).ToListAsync();

            if (learnedWordsSinceOneWeekIds == null || learnedWordsSinceOneWeekIds.Count == 0)
            {
                return null;
            }

            await Queryable().Where(x => learnedWordsSinceOneWeekIds.Contains(x.Id))
            .ExecuteUpdateAsync(s =>
                s.SetProperty(n => n.IsLearned, n => false)
                .SetProperty(n => n.LearnedDate, n => null));

            return learnedWordsSinceOneWeekIds;
        }

        private static void ValidationupdateIsLearned(List<int> ids)
        {
            NotImplementedBusinessException.ThrowIfNull(ids, "ids Cannot Be Empty");
        }

        private static int GetTrueAnswerCount(WordResponse unlearnedWord)
        {
            int trueAnswerCount = 0;
            foreach (var wordAnswer in unlearnedWord.WordAnswers)
            {
                if (wordAnswer.Answer)
                {
                    trueAnswerCount++;
                }
                else
                {
                    break;
                }
            }

            return trueAnswerCount;
        }

    }
}
