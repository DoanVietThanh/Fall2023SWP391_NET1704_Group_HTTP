using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class CourseReservationService : ICourseReservationService
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

        public async Task<IEnumerable<CourseReservationModel>> GetAllByCourseId(Guid courseId)
        {
            return await _courseReservationRepo.GetAllByCourseId(courseId);
        }

        public async Task<CourseReservationModel> GetByMemberAsync(Guid memberId)
        {
            return await _courseReservationRepo.GetByMemberAsync(memberId);
        }

        public async Task<int> GetTotalMemberByMentorId(Guid mentorId)
        {
            return await _courseReservationRepo.GetTotalMemberByMentorId(mentorId);
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid id)
        {
            return await _courseReservationRepo.UpdatePaymentStatusAsync(id);
        }
    }
}
