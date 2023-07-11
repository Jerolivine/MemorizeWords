using MemorizeWords.Application.Word.Interfaces;
using MemorizeWords.Infrastructure.Presentation;
using MemorizeWords.Presentation.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace MemorizeWords.Presentation
{
    public class WordAnswerController : BaseController
    {
        public IWordAnswerService _wordAnswerService { get; set; }
        public WordAnswerController(IWordAnswerService wordAnswerService)
        {
            _wordAnswerService = wordAnswerService;
        }

        [HttpPost("answer")]
        public async Task<IResult> AnswerAsync([FromBody]WordAnswerRequest wordAnswerRequest)
        {
            var answerResponse = await _wordAnswerService.AnswerAsync(wordAnswerRequest);
            return Results.Ok(answerResponse);
        }
    }
}
