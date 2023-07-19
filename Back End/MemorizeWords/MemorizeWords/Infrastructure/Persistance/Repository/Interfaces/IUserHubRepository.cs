using MemorizeWords.Entity;

namespace MemorizeWords.Infrastructure.Persistance.Repository.Interfaces
{
    public interface IUserHubRepository
    {
        Task UpdateUserHubAsnyc(int wordAnswerId);
        Task<UserHubEntity> GetUserHubAsync();
    }
}
