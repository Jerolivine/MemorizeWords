using MemorizeWords.Presentation.Models.Dto;

namespace MemorizeWords.Presentation.Models.Response
{
    public class WordResponse
    {
        public int WordId { get; set; }
        public string Word { get; set; }
        public string Meaning { get; set; }
        public string WritingInLanguage { get; set; }
        public string Percentage { get; set; }
        public List<WordAnswerDto> WordAnswers { get; set; }
    }
}
