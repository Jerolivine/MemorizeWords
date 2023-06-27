using MemorizeWords.Context.EFCore;
using MemorizeWords.Entity;
using MemorizeWords.Models.Dto;
using MemorizeWords.Models.Request;
using MemorizeWords.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace MemorizeWords.Api.Apis
{
    public class WordApiInitializer : IInitializer
    {
        public void Initialize(WebApplication app)
        {
            app.MapPost("/word", async (WordAddRequest wordAddRequest, MemorizeWordsDbContext memorizeWordsDbContext) =>
            {
                ValidationAddUpdateWord(wordAddRequest);

                var wordEntity = await memorizeWordsDbContext.Word.FirstOrDefaultAsync(x => x.Word.Equals(wordAddRequest.Word.ToUpper()));
                if (wordEntity != null)
                {
                    wordEntity.Meaning = wordAddRequest.Meaning;
                    wordEntity.WritingInLanguage = wordEntity.WritingInLanguage;
                }
                else
                {
                    wordEntity = new WordEntity() { Word = wordAddRequest.Word, Meaning = wordAddRequest.Meaning,WritingInLanguage = wordAddRequest.WritingInLanguage };
                    memorizeWordsDbContext.Add(wordEntity);
                }

                await memorizeWordsDbContext.SaveChangesAsync();

                return Results.Created($"/word/{wordEntity.Id}", wordEntity);
            });

            app.MapPost("/answer", async (WordAnswerRequest wordAnswerRequest, MemorizeWordsDbContext memorizeWordsDbContext, IConfiguration configuration) =>
            {
                ValidationAnswer(wordAnswerRequest, memorizeWordsDbContext);

                WordEntity wordEntity;
                bool isAnswerTrue = GetGivenAnswer(wordAnswerRequest, memorizeWordsDbContext, out wordEntity);

                memorizeWordsDbContext.WordAnswer.Add(new WordAnswerEntity()
                {
                    WordId = wordAnswerRequest.WordId,
                    Answer = isAnswerTrue,
                    AnswerDate = DateTime.Now,
                });

                await memorizeWordsDbContext.SaveChangesAsync();

                if (isAnswerTrue)
                {
                    await UpdateIsLearnedIfItIsLearned(wordAnswerRequest.WordId, memorizeWordsDbContext, configuration);
                }

                return Results.Ok(new AnswerResponse()
                {
                    IsAnswerTrue = isAnswerTrue,
                    Meaning = wordEntity.Meaning
                });
            });

            app.MapGet("/unlearnedWords", async (MemorizeWordsDbContext memorizeWordsDbContext, IConfiguration configuration) =>
            {

                int sequentTrueAnswerCount = GetSequentTrueAnswerCount(configuration);
                var result = await memorizeWordsDbContext.Word.Where(x => x.IsLearned == false)
                            .Select(x => new
                            {
                                WordId = x.Id,
                                Meaning = x.Meaning,
                                Word = x.Word,
                                WritingInLanguage = x.WritingInLanguage,
                                WordAnswers = x.WordAnswers.OrderByDescending(x => x.AnswerDate).Take(sequentTrueAnswerCount).Select(x => new WordAnswerDto()
                                {
                                    Answer = x.Answer,
                                    Id = x.Id
                                }).ToList()
                            })
                            .Select(p => new WordResponse()
                            {
                                WordId = p.WordId,
                                Meaning = p.Meaning,
                                Word = p.Word,
                                WordAnswers = p.WordAnswers,
                                WritingInLanguage = p.WritingInLanguage,
                                Percentage = ((((double)p.WordAnswers.Where(x => x.Answer).Count() / sequentTrueAnswerCount)) * 100).ToString()
                            })
                            .ToListAsync();

                return Results.Ok(result);
            });

            app.MapGet("/learnedWords", (MemorizeWordsDbContext memorizeWordsDbContext) =>
            {

                var result = memorizeWordsDbContext.Word.Where(x => x.IsLearned)
                            .Select(p => new WordResponse()
                            {
                                WordId = p.Id,
                                Meaning = p.Meaning,
                                Word = p.Word,
                                WritingInLanguage = p.WritingInLanguage
                            })
                            .ToList();

                return Results.Ok(result);
            });

            app.MapGet("/questionWords", (MemorizeWordsDbContext memorizeWordsDbContext) =>
            {

                var randomWords = memorizeWordsDbContext.Word.Where(x => x.IsLearned == false)
                                   .OrderBy(x => Guid.NewGuid())
                                   .Take(20)
                                   .Select(x => new QuestionWordResponse()
                                   {
                                       Word = x.Word,
                                       Id = x.Id
                                   });

                return Results.Ok(randomWords);
            });

        }

        private void ValidationAddUpdateWord(WordAddRequest wordAddRequest)
        {
            ArgumentNullException.ThrowIfNull(wordAddRequest, "Request Cannot Be Empty");
            ArgumentNullException.ThrowIfNull(wordAddRequest?.Word, "Word Cannot Be Empty");
            ArgumentNullException.ThrowIfNull(wordAddRequest?.Meaning, "Meaning Cannot Be Empty");
        }

        private bool GetGivenAnswer(WordAnswerRequest wordAnswerRequest, MemorizeWordsDbContext memorizeWordsDbContext, out WordEntity wordEntity)
        {
            wordEntity = memorizeWordsDbContext.Word.FirstOrDefault(x => x.Id == wordAnswerRequest.WordId);

            bool answer = wordEntity.Meaning.ToUpper().Equals(wordAnswerRequest.GivenAnswerMeaning.ToUpper());
            return answer;
        }

        private void ValidationAnswer(WordAnswerRequest wordAnswerRequest, MemorizeWordsDbContext memorizeWordsDbContext)
        {
            ArgumentNullException.ThrowIfNull(wordAnswerRequest, "Request Cannot Be Empty");
            ArgumentNullException.ThrowIfNull(wordAnswerRequest?.WordId, "WordId Cannot Be Empty");
            ArgumentNullException.ThrowIfNull(wordAnswerRequest?.GivenAnswerMeaning, "GivenAnswerMeaning Cannot Be Empty");

            var wordEntity = memorizeWordsDbContext.Word.FirstOrDefault(x => x.Id == wordAnswerRequest.WordId);
            ArgumentNullException.ThrowIfNull(wordEntity, $"Word Couldnt found by given Id, {wordAnswerRequest.WordId}");
        }

        private async Task UpdateIsLearnedIfItIsLearned(int wordId, MemorizeWordsDbContext memorizeWordsDbContext, IConfiguration configuration)
        {
            int sequentTrueAnswerCount = GetSequentTrueAnswerCount(configuration);
            var answers = await memorizeWordsDbContext.WordAnswer.Where(x => x.WordId == wordId).OrderByDescending(x => x.AnswerDate).Take(sequentTrueAnswerCount).ToListAsync();
            if (answers?.Count != sequentTrueAnswerCount)
            {
                return;
            }

            if (answers.Any(x => !x.Answer))
            {
                return;
            }

            var wordEntityGivenAnswer = await memorizeWordsDbContext.Word.FirstOrDefaultAsync(x => x.Id == wordId);
            wordEntityGivenAnswer.IsLearned = true;

            await memorizeWordsDbContext.SaveChangesAsync();
        }

        private int GetSequentTrueAnswerCount(IConfiguration configuration)
        {
            int sequentTrueAnswerCount;
            int.TryParse(configuration["SequentTrueAnswerCount"], out sequentTrueAnswerCount);

            return sequentTrueAnswerCount;
        }
    }
}
