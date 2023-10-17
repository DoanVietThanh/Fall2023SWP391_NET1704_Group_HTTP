using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ICourseReservationService
    {
        Task<CourseReservationModel> CreateAsync(CourseReservationModel courseReservation);
        Task<CourseReservationModel> GetByMemberAsync(Guid memberId);
        Task<IEnumerable<CourseReservationModel>> GetAllByCourseId(Guid courseId);
        Task<int> GetTotalMemberByMentorId(Guid mentorId);
        Task<bool> UpdatePaymentStatusAsync(Guid id);
    }
}
