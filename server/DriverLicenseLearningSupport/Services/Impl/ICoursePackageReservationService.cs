using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ICoursePackageReservationService
    {
        Task<CoursePackageReservationModel> CreateAsync(CoursePackageReservationModel courseReservation);
        Task<CoursePackageReservationModel> GetByMemberAsync(Guid memberId);
        Task<IEnumerable<CoursePackageReservationModel>> GetAllByCourseIdAsync(Guid courseId);
        Task<IEnumerable<CoursePackageReservationModel>> GetAllAsync();
        Task<IEnumerable<MemberModel>> GetAllMemberInCourseAsync(Guid courseId);
        Task<int> GetTotalMemberByMentorIdAsync(Guid mentorId);
        Task<bool> UpdatePaymentStatusAsync(Guid id, double paymentAmmount);
    }
}
