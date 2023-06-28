using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MemorizeWords.Infrastructure.Transversal.Exception.Exceptions
{
    public class NotImplementedBusinessException : System.Exception
    {
        public NotImplementedBusinessException(string message) : base(message)
        {

        }

        [DoesNotReturn]
        public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
        {
            if (argument is null)
            {
                throw new NotImplementedBusinessException(paramName);
            }
        }
    }
}
