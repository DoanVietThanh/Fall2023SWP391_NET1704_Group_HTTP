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

        public async Task<IEnumerable<CoursePackageReservationModel>> GetAllByCourseIdAsync(Guid courseId)
        {
            return await _courseReservationRepo.GetAllByCourseIdAsync(courseId);
        }

        public async Task<IEnumerable<MemberModel>> GetAllMemberInCourseAsync(Guid courseId)
        {
            return await _courseReservationRepo.GetAllMemberInCourseAsync(courseId);
        }

        public async Task<CoursePackageReservationModel> GetByMemberAsync(Guid memberId)
        {
            return await _courseReservationRepo.GetByMemberAsync(memberId);
        }
        public async Task<int> GetTotalMemberByMentorIdAsync(Guid mentorId)
        {
            return await _courseReservationRepo.GetTotalMemberByMentorIdAsync(mentorId);
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid id, double paymentAmmount)
        {
            return await _courseReservationRepo.UpdatePaymentStatusAsync(id, paymentAmmount);
        }
    }
}
