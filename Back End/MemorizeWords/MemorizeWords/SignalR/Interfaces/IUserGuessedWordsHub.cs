namespace MemorizeWords.SignalR.Interfaces
{
    public interface IUserGuessedWordsHub
    {
        Task ReceiveUserGuessedWords(string message);
    }
}
