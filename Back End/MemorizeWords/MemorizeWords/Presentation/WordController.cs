using MemorizeWords.Application.Word.Interfaces;
using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Presentation;
using MemorizeWords.Infrastructure.Transversal.AppLog.Interfaces;
using MemorizeWords.Presentation.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace MemorizeWords.Presentation
{
    public class WordController : BaseController
    {
        public IWordService _wordService { get; set; }
        public IWordAnswerService _wordAnswerService { get; set; }
        public IApplicationLogger _logger { get; set; }

        public WordController(IWordService wordService,
            IWordAnswerService wordAnswerService,
            IApplicationLogger logger)
        {
            _wordService = wordService;
            _wordAnswerService = wordAnswerService;
            _logger = logger;
        }

        [HttpPost()]
        public async Task<IResult> AddWordAsync([FromBody]WordAddRequest wordAddRequest)
        {
            var wordEntity = await _wordService.AddWordAsync(wordAddRequest);
            return Results.Created($"/word/{wordEntity.Id}", wordEntity);
        }

        [HttpGet("question-words")]
        public async Task<IResult> GetQuestionWordsAsync()
        {
            var questionWords = await _wordService.GetQuestionWordsAsync();
            return Results.Ok(questionWords);
        }

        [HttpGet("unlearned-words")]
        public async Task<IResult> UnLearnedWordsAsync()
        {
            var unlearnedWords = await _wordService.UnLearnedWordsAsync();
            return Results.Ok(unlearnedWords);
        }

        [HttpGet("learned-words")]
        public async Task<IResult> LearnedWordsAsync()
        {
            var learnedWords = await _wordService.LearnedWordsAsync();
            return Results.Ok(learnedWords);
        }

        [HttpPost("update-is-learned")]
        public async Task<IResult> UpdateIsLearnedAsync([FromBody] WordUpdateIsLearnedRequest wordUpdateIsLearnedRequest)
        {
            await _wordService.UpdateIsLearnedAsync(wordUpdateIsLearnedRequest);

            if (wordUpdateIsLearnedRequest.IsLearned)
            {
                await _wordAnswerService.DeleteAllAnswersAsync(wordUpdateIsLearnedRequest.Ids);
            }

            return Results.Ok(wordUpdateIsLearnedRequest.Ids);
        }

    }
}
