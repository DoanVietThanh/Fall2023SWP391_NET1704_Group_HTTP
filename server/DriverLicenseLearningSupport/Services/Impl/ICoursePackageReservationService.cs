using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ICoursePackageReservationService
    {
        Task<CoursePackageReservationModel> CreateAsync(CoursePackageReservationModel courseReservation);
        Task<CoursePackageReservationModel> GetByMemberAsync(Guid memberId);
        Task<IEnumerable<CoursePackageReservationModel>> GetAllByCourseId(Guid courseId);
        Task<int> GetTotalMemberByMentorId(Guid mentorId);
        Task<bool> UpdatePaymentStatusAsync(Guid id, double paymentAmmount);
    }
}
