using MemorizeWords.Entity;

namespace MemorizeWords.Infrastructure.Persistance.Repository.Interfaces
{
    public interface IUserHubConnectionRepository
    {
        Task UpdateUserHubAsync(int userId, string hubContext);
        Task<List<UserHubConnectionEntity>> GetUsersHub(List<int> userIds);
        Task DeleteUser(int userId);
        Task<bool> IsAnyUserConnectedToHub();
    }
}
