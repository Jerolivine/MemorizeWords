using MemorizeWords.Infrastructure.Extensions;
using MemorizeWords.Infrastructure.Presentation;
using MemorizeWords.Presentation.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace MemorizeWords.Presentation
{
    public class ApplicationVariablesController : BaseController
    {
        private readonly IConfiguration _configuration;

        public ApplicationVariablesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("app-variables")]
        public IResult GetApplicationVariables()
        {
            int sequentTrueAnswerCount = _configuration.GetSequentTrueAnswerCount();

            var applicationVariables = new ApplicationVariableResponse
            {
                SequentTrueAnswerCount = sequentTrueAnswerCount
            };

            return Results.Ok(applicationVariables);
        }
    }
}