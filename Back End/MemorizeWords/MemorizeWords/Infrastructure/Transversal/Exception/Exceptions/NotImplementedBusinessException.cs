using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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
    }
}
