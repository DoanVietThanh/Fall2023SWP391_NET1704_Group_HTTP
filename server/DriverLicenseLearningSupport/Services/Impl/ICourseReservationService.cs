using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ICourseReservationService
    {
        Task<CourseReservationModel> CreateAsync(CourseReservationModel courseReservation);
        Task<CourseReservationModel> GetByMemberAsync(Guid memberId);
    }
}
