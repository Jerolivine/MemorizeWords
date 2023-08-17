using MemorizeWords.Application.UserGuessedWords.Interfaces;
using MemorizeWords.Application.Word.Interfaces;
using MemorizeWords.Infrastructure.Presentation;
using MemorizeWords.Presentation.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace MemorizeWords.Presentation
{
    public class WordController : BaseController
    {
        public IWordService _wordService { get; set; }
        public IUserGuessedWordsService _userGuessedWords { get; set; }
        public WordController(IWordService wordService,
            IUserGuessedWordsService userGuessedWords)
        {
            _wordService = wordService;
            _userGuessedWords = userGuessedWords;
        }

        [HttpPost()]
        public async Task<IResult> AddWordAsync([FromBody] WordAddRequest wordAddRequest)
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

            return Results.Ok(wordUpdateIsLearnedRequest.Ids);
        }

        [HttpPost("delete")]
        public async Task<IResult> DeleteAsync([FromBody] List<int> ids)
        {
            await _wordService.DeleteAsync(ids);
            return Results.Ok();
        }

        [HttpPost("dont-ask-this-word")]
        public async Task<IResult> DontAskThisWord([FromBody] List<int> ids)
        {
            await _wordService.DontAskThisWord(ids);
            return Results.Ok();
        }

    }
}
