using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IPracticeExamRepository
    {
        public Task<IEnumerable<PracticeExam>> GetAllPracticeExamAsync();

        public Task<PracticeExamModel> GetPracticeExamAsync(int practiceExamId);
        public Task<bool> CreatePracticeExamAsync(PracticeExam practiceExam);

        public Task DeletePraticeExamAsynx(int practiceExamId);

        public Task UpdatePracticeExamAsync(int practiceExamId, PracticeExamModel updatePracticeExam);


    }
}
