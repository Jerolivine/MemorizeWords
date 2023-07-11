namespace MemorizeWords.Presentation.Models.Request
{
    public record WordUpdateIsLearnedRequest(List<int> Ids, bool IsLearned);

}
