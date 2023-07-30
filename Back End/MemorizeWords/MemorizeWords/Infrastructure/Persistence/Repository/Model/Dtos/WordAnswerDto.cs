namespace MemorizeWords.Infrastructure.Persistance.Repository.Model.Dtos
{
    public class WordAnswerDto
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public bool Answer { get; set; }
        public DateTime AnswerDate { get; set; }
        public string Percentage { get; set; }
    }
}
