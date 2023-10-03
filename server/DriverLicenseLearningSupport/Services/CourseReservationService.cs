using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class CourseReservationService : ICourseServationService
    {
        private readonly ICourseReservationRepository _courseReservationRepo;
        private readonly IMapper _mapper;

        public CourseReservationService(ICourseReservationRepository courseReservationRepo,
            IMapper mapper)
        {
            _courseReservationRepo = courseReservationRepo;
            _mapper = mapper;
        }

        public async Task<CourseReservationModel> CreateAsync(CourseReservationModel courseReservation)
        {
            var courseReservationEntity = _mapper.Map<CourseReservation>(courseReservation);
            return await _courseReservationRepo.CreateAsync(courseReservationEntity);
        }
    }
}
