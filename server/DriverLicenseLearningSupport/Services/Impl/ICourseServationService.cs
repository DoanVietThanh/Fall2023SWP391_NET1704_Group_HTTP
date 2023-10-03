using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ICourseServationService
    {
        Task<CourseReservationModel> CreateAsync(CourseReservationModel courseReservation);
    }
}
