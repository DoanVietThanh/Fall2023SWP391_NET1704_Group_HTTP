using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ICourseReservationRepository
    {
        Task<CourseReservationModel> CreateAsync(CourseReservation courseReservation);
        Task<CourseReservationModel> GetByMemberAsync(Guid memberId);
        Task<IEnumerable<CourseReservationModel>> GetAllByCourseId(Guid courseId);
        Task<int> GetTotalMemberByMentorId(Guid mentorId);
        Task<bool> UpdatePaymentStatusAsync(Guid id);
    }
}
