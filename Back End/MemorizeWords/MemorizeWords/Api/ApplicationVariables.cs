using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemorizeWords.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationVariables : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ApplicationVariables(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetApplicationVariables()
        {
            var sequentTrueAnswerCount = _configuration.GetValue<int>("SequentTrueAnswerCount");

            var applicationVariables = new
            {
                SequentTrueAnswerCount = sequentTrueAnswerCount
            };

            return Ok(applicationVariables);
        }
    }
}
