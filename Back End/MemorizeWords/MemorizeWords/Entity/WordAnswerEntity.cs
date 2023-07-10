using MemorizeWords.Infrastructure.Entity.Core.Interfaces;

namespace MemorizeWords.Entity
{
    public class WordAnswerEntity : IEntity<int>
    {
        public int Id { get; set; }
        public bool Answer { get; set; }
        public DateTime AnswerDate { get; set; }

        public int WordId { get; set; }
        public WordEntity Word { get; set; }
    }
}
