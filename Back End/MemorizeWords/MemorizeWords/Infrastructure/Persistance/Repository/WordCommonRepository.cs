using MemorizeWords.Infrastructure.Persistance.Interfaces;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using MemorizeWords.Presentation.Models.Response;

namespace MemorizeWords.Infrastructure.Persistance.Repository
{
    public class WordCommonRepository : IWordCommonRepository, IBusinessRepository
    {
        public int GetTrueAnswerCount(WordResponse unlearnedWord)
        {
            int trueAnswerCount = 0;
            foreach (var wordAnswer in unlearnedWord.WordAnswers)
            {
                if (wordAnswer.Answer)
                {
                    trueAnswerCount++;
                }
                else
                {
                    break;
                }
            }

            return trueAnswerCount;
        }
    }
}
