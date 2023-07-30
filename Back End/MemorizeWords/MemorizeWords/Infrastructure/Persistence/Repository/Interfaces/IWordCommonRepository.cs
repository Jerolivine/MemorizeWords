using MemorizeWords.Presentation.Models.Response;

namespace MemorizeWords.Infrastructure.Persistance.Repository.Interfaces
{
    public interface IWordCommonRepository
    {
        int GetTrueAnswerCount(WordResponse unlearnedWord);
    }
}
