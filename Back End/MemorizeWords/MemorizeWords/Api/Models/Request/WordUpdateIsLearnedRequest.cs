namespace MemorizeWords.Api.Models.Request
{
    public record WordUpdateIsLearnedRequest(List<int> Ids, bool IsLearned);

}
