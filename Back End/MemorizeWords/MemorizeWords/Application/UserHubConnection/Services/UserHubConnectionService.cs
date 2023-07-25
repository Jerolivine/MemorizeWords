using MemorizeWords.Application.UserHubConnection.Interfaces;
using MemorizeWords.Infrastructure.Application.Interfaces;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;

namespace MemorizeWords.Application.UserHubConnection.Services
{
    public class UserHubConnectionService : IUserHubConnectionService ,IBusinessService
    {
        public IUserHubConnectionRepository _userHubConnectionRepository { get; set; }

        public UserHubConnectionService(IUserHubConnectionRepository userHubConnectionRepository)
        {
            _userHubConnectionRepository = userHubConnectionRepository;
        }

        public async Task UpdateUserConnection(int userId,string hubContextId)
        {
            await _userHubConnectionRepository.UpdateUserHubAsync(userId, hubContextId);
        }

        public async Task DeleteUser(int userId)
        {
            await _userHubConnectionRepository.DeleteUser(userId);
        }

    }
}
