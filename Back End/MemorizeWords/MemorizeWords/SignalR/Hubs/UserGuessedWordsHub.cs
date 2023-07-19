using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Extensions;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using MemorizeWords.SignalR.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace MemorizeWords.SignalR.Hubs
{
    public class UserGuessedWordsHub : Hub<IUserGuessedWordsHub>
    {
        private static readonly ConcurrentDictionary<string, string> activeGroups = new ConcurrentDictionary<string, string>();
        public IUserHubRepository _userHubRepository { get; set; }
        public IWordAnswerRepository _wordAnswerRepository { get; set; }

        public UserGuessedWordsHub(IUserHubRepository userHubRepository
            , IWordAnswerRepository wordAnswerRepository)
        {
            _userHubRepository = userHubRepository;
            _wordAnswerRepository = wordAnswerRepository;
        }

        public override async Task OnConnectedAsync()
        {

            // TODO-Arda : Token Handler
            var userId = Context.GetHttpContext().Request.Headers["UserId"].ToString();

            if (userId is null)
            {
                return;
            }

            await AddNewUserToHub(userId);
            await SendClientConnectedMessage(userId);

            await base.OnConnectedAsync();
        }

        private async Task SendClientConnectedMessage(string userId)
        {
            await Clients.Group(userId).ReceiveMessageAsync($"ClientConnected {Context.ConnectionId}");
        }

        private async Task AddNewUserToHub(string userId)
        {
            if (!activeGroups.TryGetValue(userId, out _))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                activeGroups.TryAdd(Context.ConnectionId, userId);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await RemoveUserFromHub();

            await base.OnDisconnectedAsync(exception);
        }

        private async Task RemoveUserFromHub()
        {
            string userId;
            if (activeGroups.TryGetValue(Context.ConnectionId, out userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
                activeGroups.TryRemove(userId, out _);
                await SendClientDisconnectedMessage(userId);
            }
        }

        private async Task SendClientDisconnectedMessage(string userId)
        {
            await Clients.Group(userId).ReceiveMessageAsync($"ClientDisconnected {Context.ConnectionId}");
        }
    }
}
