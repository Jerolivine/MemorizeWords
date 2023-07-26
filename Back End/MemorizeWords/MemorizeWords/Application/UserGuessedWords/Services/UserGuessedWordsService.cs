using MemorizeWords.Application.UserGuessedWords.Interfaces;
using MemorizeWords.Application.UserHubConnection.Interfaces;
using MemorizeWords.Entity;
using MemorizeWords.Infrastructure.Application.Interfaces;
using MemorizeWords.Infrastructure.Extensions;
using MemorizeWords.Infrastructure.Persistance.Repository;
using MemorizeWords.Infrastructure.Persistance.Repository.Interfaces;
using MemorizeWords.Presentation.Models.Response;
using MemorizeWords.SignalR.Hubs;
using MemorizeWords.SignalR.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Quartz;

namespace MemorizeWords.Application.UserGuessedWords.Services
{
    public class UserGuessedWordsService : IUserGuessedWordsService, IBusinessService
    {
        public IHubContext<UserGuessedWordsHub, IUserGuessedWordsHub> _userGuessedWordsHub { get; set; }
        public IWordRepository _wordRepository { get; set; }
        public IWordAnswerRepository _wordAnswerRepository { get; set; }
        public IUserHubConnectionRepository _userHubConnectionRepository { get; set; }
        public IUserHubRepository _userHubRepository { get; set; }

        public UserGuessedWordsService(IHubContext<UserGuessedWordsHub, IUserGuessedWordsHub> userGuessedWordsHub,
            IWordRepository wordRepository,
            IWordAnswerRepository wordAnswerRepository,
            IUserHubConnectionRepository userHubConnectionRepository,
            IUserHubRepository userHubRepository)
        {
            _userGuessedWordsHub = userGuessedWordsHub;
            _wordRepository = wordRepository;
            _wordAnswerRepository = wordAnswerRepository;
            _userHubConnectionRepository = userHubConnectionRepository;
            _userHubRepository = userHubRepository;
        }

        public async Task PublishUserGuessedWords()
        {
            var validate = await Validate();
            if (!validate.isValid)
            {
                return;
            }

            await PublishAnswersToUsers(validate.wordAnswers);
            await UpdateHubWithLatestGivenAnswer(validate);
        }

        private async Task UpdateHubWithLatestGivenAnswer((bool isValid, List<WordAnswerEntity> wordAnswers) validate)
        {
            int newWordAnswerId = GetLatestWordAnswerId(validate.wordAnswers);
            await _userHubRepository.UpdateUserHubAsync(newWordAnswerId);
        }

        private async Task<(bool isValid, List<WordAnswerEntity> wordAnswers)> Validate()
        {
            var isAnyUserConnectedToHub = await _userHubConnectionRepository.IsAnyUserConnectedToHub();

            if (!isAnyUserConnectedToHub)
            {
                return (false, null);
            }

            List<WordAnswerEntity> wordAnswers = await GetNewGivenAnswers();

            bool hasNewAnswers = HasNewAnswers(wordAnswers);
            if (!hasNewAnswers)
            {
                return(false,null);
            }

            return (true, wordAnswers);
        }


        private async Task<List<WordAnswerEntity>> GetNewGivenAnswers()
        {
            var latestWordAnswerId = await _userHubRepository.GetLatestWordAnswerId();
            List<WordAnswerEntity> wordAnswerUserHub = await _wordAnswerRepository.GetNewGivenAnswers(latestWordAnswerId);
            return wordAnswerUserHub;
        }

        private static bool HasNewAnswers(List<WordAnswerEntity> wordAnswerUserHub)
        {
            if (wordAnswerUserHub?.Count == 0)
            {
                return false;
            }

            return true;
        }

        private static List<int> GetUserIdsFromAnswers(List<WordAnswerEntity> wordAnswerUserHub)
        {
            return wordAnswerUserHub.Select(x => x.UserId)
                                    .GroupBy(x => x)
                                    .Select(group => group.Key)
                                    .ToList();
        }

        private async Task PublishAnswersToUsers(List<WordAnswerEntity> wordAnswers)
        {
            List<int> userIds = GetUserIdsFromAnswers(wordAnswers);

            var userHubs = await _userHubConnectionRepository.GetUsersHub(userIds);

            RemoveUserIdsNotInHub(userIds, userHubs);

            // TODO-Arda:  Task.WaitAll
            foreach (var userId in userIds)
            {
                List<WordResponse> userAnswers = new();
                var wordIds = GetWordIds(wordAnswers, userId);

                var userWordAnswers = await _wordRepository.GetWordAnswersHub(wordIds);
                userAnswers.AddRange(userWordAnswers);

                await PublishAnswersToUser(userId.ToString(), userAnswers);

            }
        }

        private static void RemoveUserIdsNotInHub(List<int> userIds, List<UserHubConnectionEntity> userHubs)
        {
            var userIdsOnHub = userHubs.Select(x => x.UserId).ToList();

            userIds.RemoveAll(item => !userIdsOnHub.Contains(item));
        }

        private async Task PublishAnswersToUser(string userId, List<WordResponse> userAnswers)
        {
            if (userAnswers?.Count == 0)
            {
                return;
            }

            await _userGuessedWordsHub.Clients.Group(userId).ReceiveUserGuessedWords(userAnswers.ToCamelCaseJson());
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
