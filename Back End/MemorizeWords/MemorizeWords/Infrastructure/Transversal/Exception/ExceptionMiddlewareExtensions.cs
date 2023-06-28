namespace MemorizeWords.Infrastructure.Transversal.Exception
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder AddExceptionMiddieware(this WebApplication app)
            => app.UseMiddleware<ExceptionMiddleware>();
    }
}
