using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class CoursePackageReservationService : ICoursePackageReservationService
    {
        private readonly ICoursePackageReservationRepository _courseReservationRepo;
        private readonly IMapper _mapper;

        public CoursePackageReservationService(ICoursePackageReservationRepository courseReservationRepo,
            IMapper mapper)
        {
            _courseReservationRepo = courseReservationRepo;
            _mapper = mapper;
        }

        public async Task<CoursePackageReservationModel> CreateAsync(CoursePackageReservationModel courseReservation)
        {
            var courseReservationEntity = _mapper.Map<CoursePackageReservation>(courseReservation);
            return await _courseReservationRepo.CreateAsync(courseReservationEntity);
        }

        public async Task<IEnumerable<CoursePackageReservationModel>> GetAllByCourseId(Guid courseId)
        {
            return await _courseReservationRepo.GetAllByCourseId(courseId);
        }

        public async Task<CoursePackageReservationModel> GetByMemberAsync(Guid memberId)
        {
            return await _courseReservationRepo.GetByMemberAsync(memberId);
        }

        public async Task<int> GetTotalMemberByMentorId(Guid mentorId)
        {
            return await _courseReservationRepo.GetTotalMemberByMentorId(mentorId);
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid id, double paymentAmmount)
        {
            return await _courseReservationRepo.UpdatePaymentStatusAsync(id, paymentAmmount);
        }
    }
}
