using MemorizeWords.Infrastructure.Transversal.Exception.Exceptions;

namespace MemorizeWords.Api.Apis
{
    public class ExceptionApiInitializer : IInitializer
    {
        public void Initialize(WebApplication app)
        {
            app.MapGet("/businessException", () =>
            {
                throw new BusinessException("BusinessException");
            });
        }
    }
}
