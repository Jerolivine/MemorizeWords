using MemorizeWords.Infrastructure.Persistence.Repository.Interfaces;
using MemorizeWords.SignalR.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace MemorizeWords.SignalR.Hubs
{
    public class UserGuessedWordsHub : Hub<IUserGuessedWordsHub>
    {
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
            var userId = Context?.GetHttpContext()?.Request.Headers["UserId"].ToString();

            if(userId is null)
            {
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            await Clients.Group(userId).ReceiveMessageAsync($"ClientConnected {Context.ConnectionId}");

            await base.OnConnectedAsync();
        }
    }
}
