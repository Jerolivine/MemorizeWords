using Microsoft.Extensions.Configuration;

namespace MemorizeWords.Api
{
    public static class ApplicationVariableApiInitializer
    {
        public static void Initialize(WebApplication app, IConfiguration configuration) {

            app.MapGet("/api/ApplicationVariable", () =>
            {
                var sequentTrueAnswerCount = configuration.GetValue<int>("SequentTrueAnswerCount");

                var applicationVariables = new
                {
                    SequentTrueAnswerCount = sequentTrueAnswerCount
                };

                return Results.Ok(applicationVariables);
            });

        }
    }
}
