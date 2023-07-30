using System.Diagnostics.CodeAnalysis;

namespace MemorizeWords.Infrastructure.Transversal.Exception.Exceptions
{
    [Serializable]
    public class NotImplementedBusinessException : BusinessException
    {
        public NotImplementedBusinessException(string message) : base(message)
        {

        }

        [DoesNotReturn]
        public static void ThrowIfNull([NotNull] object argument,  string message = null)
        {
            if (argument is null)
            {
                throw new NotImplementedBusinessException(message);
            }
        }

        [DoesNotReturn]
        public static void ThrowIfNull<T>([NotNull] IEnumerable<T> argument, string message = null)
        {
            ThrowIfNull(argument, message);

            if (!argument.Any())
            {
                throw new NotImplementedBusinessException(message);
            }
        }
    }
}
