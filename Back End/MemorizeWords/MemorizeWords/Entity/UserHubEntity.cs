using MemorizeWords.Infrastructure.Entity.Core.Interfaces;

namespace MemorizeWords.Entity
{
    public class UserHubEntity : IEntity<int>
    {
        public int Id { get; set; }
        public int WordAnswerId { get; set; }
    }
}
