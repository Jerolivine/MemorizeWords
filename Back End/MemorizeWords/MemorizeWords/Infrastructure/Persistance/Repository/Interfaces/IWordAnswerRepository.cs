﻿using MemorizeWords.Entity;
using MemorizeWords.Presentation.Models.Request;
using MemorizeWords.Presentation.Models.Response;

namespace MemorizeWords.Infrastructure.Persistance.Repository.Interfaces
{
    public interface IWordAnswerRepository
    {
        Task<AnswerResponse> AnswerAsync(WordAnswerRequest wordAnswerRequest);
        Task DeleteAllAnswersAsync(List<int> wordIds);
        Task LeaveEnoughTrueAnswerToMemorize(List<int> wordIds);
        Task<List<WordAnswerEntity>> GetWordAnswersHub(int? id);
        Task<List<WordAnswerEntity>> GetAnswersOfUserIdAsync(int wordId, int userId);
    }
}
