namespace MemorizeWords.Infrastructure.Transversal.Exception.Exceptions
{
    [Serializable]
    public class BusinessException : System.Exception
    {
        public BusinessException(string userMessage) : base(userMessage)
        { 

        }
    }
}
