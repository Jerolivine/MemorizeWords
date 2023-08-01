namespace MemorizeWords.Presentation.Models.Request
{
    public class WordUpdateIsLearnedRequest
    {
        public List<int> Ids { get; set; }
        public bool IsLearned { get; set; }
    }

}
