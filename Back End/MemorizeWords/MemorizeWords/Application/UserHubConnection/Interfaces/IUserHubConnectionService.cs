namespace MemorizeWords.Application.UserHubConnection.Interfaces
{
    public interface IUserHubConnectionService
    {
        Task UpdateUserConnection(int userId, string hubContextId);
        Task DeleteUser(int userId);
    }
}
