using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MemorizeWords.Infrastructure.Transversal.Exception.Exceptions
{
    [Serializable]
    public class KeyNotFoundBusinessException : BusinessException
    {
        public KeyNotFoundBusinessException(string message) : base(message)
        {

        }

       
    }
}
