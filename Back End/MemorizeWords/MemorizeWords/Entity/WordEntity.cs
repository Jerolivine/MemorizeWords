using MemorizeWords.Entity.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemorizeWords.Entity
{
    public class WordEntity : IEntity<int>
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Meaning { get; set; }
        public bool IsLearned { get; set; }

        public ICollection<WordAnswerEntity> WordAnswers{ get; set; }
    }
}
