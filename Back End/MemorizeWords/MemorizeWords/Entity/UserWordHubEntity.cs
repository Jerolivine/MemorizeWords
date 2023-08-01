﻿using MemorizeWords.Infrastructure.Entity.Core.Interfaces;

namespace MemorizeWords.Entity
{
    public class UserWordHubEntity : IEntity<int>
    {
        public int Id { get; set; }
        public int WordAnswerId { get; set; }
    }
}
