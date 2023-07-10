//using MemorizeWords.Api.Models.Dto;
//using MemorizeWords.Api.Models.Request;
//using MemorizeWords.Api.Models.Response;
//using MemorizeWords.Entity;
//using MemorizeWords.Infrastructure.Persistance.FCore.Context;
//using MemorizeWords.Infrastructure.Transversal.Exception.Exceptions;
//using Microsoft.EntityFrameworkCore;

//namespace MemorizeWords.Api.Apis
//{
//    public class WordApiInitializer : IInitializer
//    {
//        public void Initialize(WebApplication app)
//        {
//            app.MapPost("/word", async (WordAddRequest wordAddRequest, EFCoreDbContext memorizeWordsDbContext) =>
//            {
//                ValidationAddUpdateWord(wordAddRequest);

//                var wordEntity = await memorizeWordsDbContext.Word.FirstOrDefaultAsync(x => x.Word.Equals(wordAddRequest.Word.ToUpper()));
//                if (wordEntity != null)
//                {
//                    wordEntity.Meaning = wordAddRequest.Meaning.TrimEnd();
//                    wordEntity.WritingInLanguage = wordEntity.WritingInLanguage.TrimEnd();
//                }
//                else
//                {
//                    wordEntity = new WordEntity() { Word = wordAddRequest.Word.TrimEnd(), Meaning = wordAddRequest.Meaning.TrimEnd(), WritingInLanguage = wordAddRequest.WritingInLanguage.TrimEnd() };
//                    memorizeWordsDbContext.Add(wordEntity);
//                }

//                await memorizeWordsDbContext.SaveChangesAsync();

//                return Results.Created($"/word/{wordEntity.Id}", wordEntity);
//            });

//            app.MapPost("/answer", async (WordAnswerRequest wordAnswerRequest, EFCoreDbContext memorizeWordsDbContext, IConfiguration configuration) =>
//            {
//                ValidationAnswer(wordAnswerRequest, memorizeWordsDbContext);

//                WordEntity wordEntity;
//                bool isAnswerTrue = GetGivenAnswer(wordAnswerRequest, memorizeWordsDbContext, out wordEntity);

//                memorizeWordsDbContext.WordAnswer.Add(new WordAnswerEntity()
//                {
//                    WordId = wordAnswerRequest.WordId,
//                    Answer = isAnswerTrue,
//                    AnswerDate = DateTime.Now,
//                });

//                await memorizeWordsDbContext.SaveChangesAsync();

//                if (isAnswerTrue)
//                {
//                    await UpdateIsLearnedIfItIsLearned(wordAnswerRequest.WordId, memorizeWordsDbContext, configuration);
//                }

//                return Results.Ok(new AnswerResponse()
//                {
//                    IsAnswerTrue = isAnswerTrue,
//                    Meaning = wordEntity.Meaning
//                });
//            });

//            app.MapGet("/unlearnedWords", async (EFCoreDbContext memorizeWordsDbContext, IConfiguration configuration) =>
//            {

//                int sequentTrueAnswerCount = GetSequentTrueAnswerCount(configuration);
//                var unlearnedWords = await memorizeWordsDbContext.Word.Where(x => x.IsLearned == false)
//                            .Select(x => new
//                            {
//                                WordId = x.Id,
//                                Meaning = x.Meaning,
//                                Word = x.Word,
//                                WritingInLanguage = x.WritingInLanguage,
//                                WordAnswers = x.WordAnswers.OrderByDescending(x => x.AnswerDate).Take(sequentTrueAnswerCount).Select(x => new WordAnswerDto()
//                                {
//                                    Answer = x.Answer,
//                                    Id = x.Id,

//                                }).ToList()
//                            })
//                            .Select(p => new WordResponse()
//                            {
//                                WordId = p.WordId,
//                                Meaning = p.Meaning,
//                                Word = p.Word,
//                                WordAnswers = p.WordAnswers,
//                                WritingInLanguage = p.WritingInLanguage,
//                                //Percentage = ((((double)p.WordAnswers.Where(x => x.Answer).Count() / sequentTrueAnswerCount)) * 100).ToString()
//                            })
//                            .ToListAsync();

//                foreach (var unlearnedWord in unlearnedWords)
//                {
//                    int trueAnswerCount = GetTrueAnswerCount(unlearnedWord);
//                    unlearnedWord.Percentage = ((((double)trueAnswerCount / sequentTrueAnswerCount)) * 100).ToString();
//                }

//                return Results.Ok(unlearnedWords);
//            });

//            app.MapGet("/learnedWords", (EFCoreDbContext memorizeWordsDbContext) =>
//            {

//                var result = memorizeWordsDbContext.Word.Where(x => x.IsLearned)
//                            .Select(p => new WordResponse()
//                            {
//                                WordId = p.Id,
//                                Meaning = p.Meaning,
//                                Word = p.Word,
//                                WritingInLanguage = p.WritingInLanguage
//                            })
//                            .ToList();

//                return Results.Ok(result);
//            });

//            app.MapGet("/questionWords", (EFCoreDbContext memorizeWordsDbContext) =>
//            {

//                var randomWords = memorizeWordsDbContext.Word.Where(x => x.IsLearned == false)
//                                   .OrderBy(x => Guid.NewGuid())
//                                   .Take(20)
//                                   .Select(x => new QuestionWordResponse()
//                                   {
//                                       Word = x.Word,
//                                       Id = x.Id,
//                                       WritingInLanguage = x.WritingInLanguage,
//                                       Meaning = x.Meaning
//                                   });

//                return Results.Ok(randomWords);
//            });

//            app.MapPost("/updateIsLearned", async (WordUpdateIsLearnedRequest wordUpdateIsLearnedRequest, EFCoreDbContext memorizeWordsDbContext) =>
//            {
//                ValidationupdateIsLearned(wordUpdateIsLearnedRequest);

//                await memorizeWordsDbContext.Word.Where(x => wordUpdateIsLearnedRequest.Ids.Contains(x.Id))
//                .ExecuteUpdateAsync(s => s.SetProperty(
//                n => n.IsLearned,
//                n => wordUpdateIsLearnedRequest.IsLearned));

//                // remove all answers
//                if (!wordUpdateIsLearnedRequest.IsLearned)
//                {
//                    await memorizeWordsDbContext.WordAnswer.Where(x => wordUpdateIsLearnedRequest.Ids.Contains(x.WordId))
//                    .ExecuteDeleteAsync();
//                }

//                return Results.Ok(wordUpdateIsLearnedRequest.Ids);
//            });

//        }

//        private static int GetTrueAnswerCount(WordResponse unlearnedWord)
//        {
//            int trueAnswerCount = 0;
//            foreach (var wordAnswer in unlearnedWord.WordAnswers)
//            {
//                if (wordAnswer.Answer)
//                {
//                    trueAnswerCount++;
//                }
//                else
//                {
//                    break;
//                }
//            }

//            return trueAnswerCount;
//        }

//        private static void ValidationupdateIsLearned(WordUpdateIsLearnedRequest wordUpdateIsLearnedRequest)
//        {
//            NotImplementedBusinessException.ThrowIfNull(wordUpdateIsLearnedRequest, "Request Cannot Be Empty");
//        }

//        private void ValidationAddUpdateWord(WordAddRequest wordAddRequest)
//        {
//            NotImplementedBusinessException.ThrowIfNull(wordAddRequest, "Request Cannot Be Empty");
//            NotImplementedBusinessException.ThrowIfNull(wordAddRequest?.Word, "Word Cannot Be Empty");
//            NotImplementedBusinessException.ThrowIfNull(wordAddRequest?.Meaning, "Meaning Cannot Be Empty");
//        }

//        private bool GetGivenAnswer(WordAnswerRequest wordAnswerRequest, EFCoreDbContext memorizeWordsDbContext, out WordEntity wordEntity)
//        {
//            wordEntity = memorizeWordsDbContext.Word.FirstOrDefault(x => x.Id == wordAnswerRequest.WordId);

//            bool answer = wordEntity.Meaning.ToUpperInvariant().Equals(wordAnswerRequest.GivenAnswerMeaning.ToUpperInvariant().ToUpper(), StringComparison.OrdinalIgnoreCase);
//            return answer;
//        }

//        private void ValidationAnswer(WordAnswerRequest wordAnswerRequest, EFCoreDbContext memorizeWordsDbContext)
//        {
//            NotImplementedBusinessException.ThrowIfNull(wordAnswerRequest, "Request Cannot Be Empty");
//            NotImplementedBusinessException.ThrowIfNull(wordAnswerRequest?.WordId, "WordId Cannot Be Empty");
//            NotImplementedBusinessException.ThrowIfNull(wordAnswerRequest?.GivenAnswerMeaning, "GivenAnswerMeaning Cannot Be Empty");

//            var wordEntity = memorizeWordsDbContext.Word.FirstOrDefault(x => x.Id == wordAnswerRequest.WordId);
//            NotImplementedBusinessException.ThrowIfNull(wordEntity, $"Word Couldnt found by given Id, {wordAnswerRequest.WordId}");
//        }

//        private async Task UpdateIsLearnedIfItIsLearned(int wordId, EFCoreDbContext memorizeWordsDbContext, IConfiguration configuration)
//        {
//            int sequentTrueAnswerCount = GetSequentTrueAnswerCount(configuration);
//            var answers = await memorizeWordsDbContext.WordAnswer.Where(x => x.WordId == wordId).OrderByDescending(x => x.AnswerDate).Take(sequentTrueAnswerCount).ToListAsync();
//            if (answers?.Count != sequentTrueAnswerCount)
//            {
//                return;
//            }

//            if (answers.Any(x => !x.Answer))
//            {
//                return;
//            }

//            var wordEntityGivenAnswer = await memorizeWordsDbContext.Word.FirstOrDefaultAsync(x => x.Id == wordId);
//            wordEntityGivenAnswer.IsLearned = true;

//            await memorizeWordsDbContext.SaveChangesAsync();
//        }

//        private int GetSequentTrueAnswerCount(IConfiguration configuration)
//        {
//            int sequentTrueAnswerCount;
//            int.TryParse(configuration["SequentTrueAnswerCount"], out sequentTrueAnswerCount);

//            return sequentTrueAnswerCount;
//        }
//    }
//}
