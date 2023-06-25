using MemorizeWords.Context.EFCore;
using MemorizeWords.Entity;
using MemorizeWords.Models.Dto;
using MemorizeWords.Models.Request;
using MemorizeWords.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace MemorizeWords.Api
{
    public static class WordApiInitializer
    {
        public static void Initialize(WebApplication app)
        {
            app.MapPost("/word", async (WordAddRequest wordAddRequest, MemorizeWordsDbContext memorizeWordsDbContext) =>
            {
                ArgumentNullException.ThrowIfNull(wordAddRequest, "Request Cannot Be Empty");
                ArgumentNullException.ThrowIfNull(wordAddRequest?.Word, "Word Cannot Be Empty");
                ArgumentNullException.ThrowIfNull(wordAddRequest?.Meaning, "Meaning Cannot Be Empty");

                var wordEntity = await memorizeWordsDbContext.Word.FirstOrDefaultAsync(x => x.Word.Equals(wordAddRequest.Word.ToUpper()));
                if (wordEntity != null)
                {
                    wordEntity.Meaning = wordAddRequest.Meaning;
                }
                else
                {
                    wordEntity = new WordEntity() { Word = wordAddRequest.Word, Meaning = wordAddRequest.Meaning };
                    memorizeWordsDbContext.Add(wordEntity);
                }

                await memorizeWordsDbContext.SaveChangesAsync();

                return Results.Created($"/word/{wordEntity.Id}", wordEntity);
            });

            app.MapPost("/answer", async (WordAnswerRequest wordAnswerRequest, MemorizeWordsDbContext memorizeWordsDbContext) =>
            {
                ArgumentNullException.ThrowIfNull(wordAnswerRequest, "Request Cannot Be Empty");
                ArgumentNullException.ThrowIfNull(wordAnswerRequest?.WordId, "WordId Cannot Be Empty");
                ArgumentNullException.ThrowIfNull(wordAnswerRequest?.GivenAnswerMeaning, "GivenAnswerMeaning Cannot Be Empty");

                var wordEntity = memorizeWordsDbContext.Word.FirstOrDefault(x => x.Id == wordAnswerRequest.WordId);
                ArgumentNullException.ThrowIfNull(wordEntity, $"Word Couldnt found by given Id, {wordAnswerRequest.WordId}");

                bool answer = wordEntity.Meaning.ToUpper().Equals(wordAnswerRequest?.GivenAnswerMeaning);

                memorizeWordsDbContext.WordAnswer.Add(new WordAnswerEntity()
                {
                    WordId = wordAnswerRequest.WordId,
                    Answer = answer,
                    AnswerDate = DateTime.Now,
                });

                await memorizeWordsDbContext.SaveChangesAsync();

                return Results.Ok();
            });

            app.MapGet("/word", (MemorizeWordsDbContext memorizeWordsDbContext) =>
            {

                var result = memorizeWordsDbContext.Word
                            .Select(p => new WordResponse()
                            {
                                WordId = p.Id,
                                Meaning = p.Meaning,
                                Word = p.Word,
                                WordAnswers = p.WordAnswers.OrderByDescending(x => x.AnswerDate).Take(10).Select(x => new WordAnswerDto()
                                {
                                    Answer = x.Answer,
                                    Id = x.Id,
                                    AnswerDate = x.AnswerDate
                                }).ToList()
                            })
                            .ToList();

                return Results.Ok(result);
            });

        }
    }
}
