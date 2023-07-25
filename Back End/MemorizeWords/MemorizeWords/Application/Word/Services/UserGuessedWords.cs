using MemorizeWords.Application.Word.Interfaces;
using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Application.Interfaces;
using MemorizeWords.Infrastructure.Extensions;
using MemorizeWords.Infrastructure.Persistence.Repository.Interfaces;
using MemorizeWords.SignalR.Hubs;
using MemorizeWords.SignalR.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace MemorizeWords.Application.Word.Services
{
    public class UserGuessedWords : IUserGuessedWords,IBusinessService
    {
        public IHubContext<UserGuessedWordsHub, IUserGuessedWordsHub> _userGuessedWordsHub { get; set; }
        public IUserHubRepository _userHubRepository { get; set; }
        public IWordAnswerRepository _wordAnswerRepository { get; set; }

        public UserGuessedWords(IHubContext<UserGuessedWordsHub, IUserGuessedWordsHub> userGuessedWordsHub,
            IUserHubRepository userHubRepository
            , IWordAnswerRepository wordAnswerRepository)
        {
            _userGuessedWordsHub = userGuessedWordsHub;
            _userHubRepository = userHubRepository;
            _wordAnswerRepository = wordAnswerRepository;
        }

        public async Task PublishUserGuessedWords()
        {

            //bool lockAcquired = Monitor.TryEnter(lockobj, FIVE_SECONDS);
            //if (!lockAcquired)
            //{
            //    return;
            //}

            UserHubEntity userHub = await _userHubRepository.GetUserHubAsync();
            List<WordAnswerEntity> wordAnswerUserHub = await _wordAnswerRepository.GetWordAnswersHub(userHub?.WordAnswerId);

            if (wordAnswerUserHub is null)
            {
                return;
            }

            int newWordAnswerId = GetLatestWordAnswerId(wordAnswerUserHub);

            List<int> userIds = GetUserIdsFromAnswers(wordAnswerUserHub);

            await PublishAnswersToUsers(wordAnswerUserHub, userIds);

            await _userHubRepository.UpdateUserHubAsnyc(newWordAnswerId);
        }

        private static List<int> GetUserIdsFromAnswers(List<WordAnswerEntity> wordAnswerUserHub)
        {
            return wordAnswerUserHub.Select(x => x.UserId)
                                    .GroupBy(x => x)
                                    .Select(group => group.Key)
                                    .ToList();
        }

        private async Task PublishAnswersToUsers(List<WordAnswerEntity> wordAnswerUserHub, List<int> userIds)
        {
            // TODO-Arda:  Task.WaitAll
            foreach (var userId in userIds)
            {
                List<WordAnswerEntity> userAnswers = new List<WordAnswerEntity>();
                var wordIds = GetWordIds(wordAnswerUserHub, userId);

                foreach (var wordId in wordIds)
                {
                    var userWordAnswers = await _wordAnswerRepository.GetAnswersOfUserIdAsync(wordId, userId);
                    userAnswers.AddRange(userWordAnswers);
                }
                await PublishAnswersToUser(userId, userAnswers);

            }
        }

        private async Task PublishAnswersToUser(int userId, List<WordAnswerEntity> userAnswers)
        {
            if(userAnswers?.Count == 0)
            {
                return;
            }

            await _userGuessedWordsHub.Clients.Group(userId.ToString()).ReceiveMessageAsync(userAnswers.ToJson());
        }

        private static int GetLatestWordAnswerId(List<WordAnswerEntity> wordAnswerUserHub)
        {
            return wordAnswerUserHub.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
        }

        private static List<int> GetWordIds(List<WordAnswerEntity> wordAnswerUserHub, int userId)
        {
            return wordAnswerUserHub.Where(x => x.UserId == userId)
                                    .GroupBy(x => x.WordId)
                                    .Select(group => group.Key)
                                    .ToList();
        }

    }
}
