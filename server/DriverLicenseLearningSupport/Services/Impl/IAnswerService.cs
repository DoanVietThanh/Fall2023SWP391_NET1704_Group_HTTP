﻿using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IAnswerService
    {
        Task<AnswerModel> CreateAsync(AnswerModel answer);
        Task<IEnumerable<AnswerModel>> CreateRangeAsync(IEnumerable<AnswerModel> answers);

        Task<IEnumerable<AnswerModel>> GetAllByQuestionId(int questionId);
        Task<bool> DeleteAnswerAsync(int id);

        Task<bool> DeleteAnswersByQuestionIdAsync(int quesitonId);
    }
}
