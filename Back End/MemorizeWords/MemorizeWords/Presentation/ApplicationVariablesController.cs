using MemorizeWords.Infrastructure.Presentation;
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
        public IActionResult GetApplicationVariables()
        {            
            // Get the value of "SequentTrueAnswerCount" from app.settings.json
            int sequentTrueAnswerCount = _configuration.GetValue<int>("SequentTrueAnswerCount");

            var applicationVariables = new
            {
                SequentTrueAnswerCount = sequentTrueAnswerCount
            };

            return Ok(applicationVariables);
        }
    }
}