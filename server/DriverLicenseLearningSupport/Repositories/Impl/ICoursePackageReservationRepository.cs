using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ICoursePackageReservationRepository
    {
        Task<CoursePackageReservationModel> CreateAsync(CoursePackageReservation courseReservation);
        Task<CoursePackageReservationModel> GetByMemberAsync(Guid memberId);
        Task<IEnumerable<CoursePackageReservationModel>> GetAllByCourseIdAsync(Guid courseId);
        Task<IEnumerable<MemberModel>> GetAllMemberInCourseAsync(Guid courseId);
        Task<int> GetTotalMemberByMentorIdAsync(Guid mentorId);
        Task<bool> UpdatePaymentStatusAsync(Guid id, double paymentAmmount);
    }
}
