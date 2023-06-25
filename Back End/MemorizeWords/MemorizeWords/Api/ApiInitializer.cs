namespace MemorizeWords.Api
{
    public static class ApiInitializer
    {
        public static void Initialize(WebApplication app)
        {
            WordApiInitializer.Initialize(app);
        }
    }
}
