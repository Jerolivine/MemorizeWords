using MemorizeWords.Application.UserHubConnection.Interfaces;
using MemorizeWords.Infrastructure.Persistence.Repository.Interfaces;
using MemorizeWords.SignalR.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace MemorizeWords.SignalR.Hubs
{
    public class UserGuessedWordsHub : Hub<IUserGuessedWordsHub>
    {
        private static readonly ConcurrentDictionary<string, string> activeGroups = new();
        public IWordAnswerRepository _wordAnswerRepository { get; set; }
        public IUserHubConnectionService _userHubConnectionService { get; set; }

        public UserGuessedWordsHub(IWordAnswerRepository wordAnswerRepository,
            IUserHubConnectionService userHubConnectionService)
        {
            _wordAnswerRepository = wordAnswerRepository;
            _userHubConnectionService = userHubConnectionService;
        }

        public override async Task OnConnectedAsync()
        {
            string userId = GetUserIdFromRequest();

            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            await AddNewUserToHub(userId);
            await SendClientConnectedMessage(userId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            LogException(exception);

            await RemoveUserFromHub();

            await base.OnDisconnectedAsync(exception);
        }

        private static void LogException(Exception exception)
        {
            if (exception is not null)
            {
                // TODO-Arda: log exception
            }
        }

        private string GetUserIdFromRequest()
        {
            // TODO-Arda : Token Handler
            return Context.GetHttpContext().Request.Query["access_token"].ToString();
        }

        private async Task SendClientConnectedMessage(string userId)
        {
            await Clients.Group(userId).ReceiveUserGuessedWords($"ClientConnected {Context.ConnectionId}");
        }

        private async Task AddNewUserToHub(string userId)
        {
            if (!activeGroups.TryGetValue(Context.ConnectionId, out _))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                activeGroups.TryAdd(Context.ConnectionId, userId);
                await _userHubConnectionService.UpdateUserConnection(int.Parse(userId), Context.ConnectionId);
            }
        }

        private async Task RemoveUserFromHub()
        {
            string userId;
            if (activeGroups.TryGetValue(Context.ConnectionId, out userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
                activeGroups.TryRemove(Context.ConnectionId, out _);
                await _userHubConnectionService.DeleteUser(int.Parse(userId));
                await SendClientDisconnectedMessage(userId);
            }

        }

        private async Task SendClientDisconnectedMessage(string userId)
        {
            await Clients.Group(userId).ReceiveUserGuessedWords($"ClientDisconnected {Context.ConnectionId}");
        }
    }
}