namespace MemorizeWords.Infrastructure.Persistance.Repository.Interfaces
{
    public interface IUserHubRepository
    {
        Task<int?> GetLatestWordAnswerId();
        Task UpdateUserHubAsync(int wordAnswerId);
    }
}
