using MemorizeWords.Models.Dto;

namespace MemorizeWords.Models.Response
{
    public class WordResponse
    {
        public int WordId { get; set; }
        public string Word { get; set; }
        public string Meaning { get; set; }
        public List<WordAnswerDto> WordAnswers { get; set; }
    }
}
