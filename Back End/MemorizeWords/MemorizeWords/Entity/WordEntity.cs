﻿using MemorizeWords.Infrastructure.Entity.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemorizeWords.Entity
{
    public class WordEntity : IEntity<int>
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Meaning { get; set; }
        public string WritingInLanguage { get; set; }
        public bool IsLearned { get; set; }
        public DateTime? LearnedDate { get; set; }

        public ICollection<WordAnswerEntity> WordAnswers{ get; set; }
    }
}
