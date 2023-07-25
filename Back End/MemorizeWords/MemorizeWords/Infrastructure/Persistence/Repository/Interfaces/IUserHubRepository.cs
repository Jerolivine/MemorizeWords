using MemorizeWords.Entity;

namespace MemorizeWords.Infrastructure.Persistence.Repository.Interfaces
{
    public interface IUserHubRepository
    {
        Task UpdateUserHubAsnyc(int wordAnswerId);
        Task<UserHubEntity> GetUserHubAsync();
    }
}
