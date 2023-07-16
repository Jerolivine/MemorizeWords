using MemorizeWords.Infrastructure.Constants;

namespace MemorizeWords.Presentation.Models.Request
{
    public record WordAnswerRequest(int WordId, string GivenAnswerMeaning, int AnswerLanguageType);

}
