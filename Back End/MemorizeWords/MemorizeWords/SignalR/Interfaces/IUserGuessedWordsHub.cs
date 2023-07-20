namespace MemorizeWords.SignalR.Interfaces
{
    public interface IUserGuessedWordsHub
    {
        Task ReceiveMessageAsync(string message);
    }
}
