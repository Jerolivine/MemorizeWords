using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MemorizeWords.Infrastructure.Transversal.Exception.Exceptions
{
    public class KeyNotFoundBusinessException : BusinessException
    {
        public KeyNotFoundBusinessException(string message) : base(message)
        {

        }

       
    }
}
