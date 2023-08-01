using MemorizeWords.Infrastructure.Entity.Core.Interfaces;

namespace MemorizeWords.Entity
{
    public class UserHubConnectionEntity : IEntity<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string HubContext { get; set; }
    }
}
