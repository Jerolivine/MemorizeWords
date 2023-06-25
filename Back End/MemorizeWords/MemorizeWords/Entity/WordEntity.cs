﻿using MemorizeWords.Entity.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemorizeWords.Entity
{
    public class WordEntity : IEntity<int>
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Word { get; set; }
        public string Meaning { get; set; }

        public ICollection<WordAnswerEntity> WordAnswers{ get; set; }
    }
}
