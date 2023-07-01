namespace MemorizeWords.Infrastructure.Configuration
{
    public static class CorsConfiguration
    {
        private static readonly string MEMORIZE_WORDS_ORIGIN = "MemorizeWordsOrigins";

        public static void ConfigureCORS(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(MEMORIZE_WORDS_ORIGIN,
                                      policy =>
                                      {
                                          policy.AllowAnyOrigin()
                                                .AllowAnyHeader()
                                                .AllowAnyMethod()
                                                //.AllowCredentials()
                                                .SetIsOriginAllowed((host) => true);
                                      });
            });
        }

        public static void ConfigureCORS(this WebApplication app)
        {
            app.UseCors(MEMORIZE_WORDS_ORIGIN);
        }
    }
}
