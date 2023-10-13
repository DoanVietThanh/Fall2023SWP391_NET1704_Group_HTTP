using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class CourseReservationRepository : ICourseReservationRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public CourseReservationRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CourseReservationModel> CreateAsync(CourseReservation courseReservation)
        {
            await _context.CourseReservations.AddAsync(courseReservation);
            bool isSucess = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!isSucess) return null;

            return _mapper.Map<CourseReservationModel>(courseReservation);
        }

        public async Task<IEnumerable<CourseReservationModel>> GetAllByCourseId(Guid courseId)
        {
            var courseReservationEntities = await _context.CourseReservations.Where(x => x.CourseId == courseId.ToString())
                                                                             .ToListAsync();
            return _mapper.Map<IEnumerable<CourseReservationModel>>(courseReservationEntities);
        }

        public async Task<CourseReservationModel> GetByMemberAsync(Guid memberId)
        {
            var courseReservationEntity = await _context.CourseReservations.Where(x => x.MemberId == memberId.ToString())
                                                                           .FirstOrDefaultAsync();
            return _mapper.Map<CourseReservationModel>(courseReservationEntity);
        }

        public async Task<int> GetTotalMemberByMentorId(Guid mentorId)
        {
            var courseReservationEntities = await _context.CourseReservations.Where(x => x.StaffId == mentorId.ToString())
                                                                             .ToListAsync();
            return courseReservationEntities.Count;
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid id)
        {
            var courseReservationEntity = await _context.CourseReservations.Where(x => x.CourseReservationId == id.ToString())
                                                                           .FirstOrDefaultAsync();

            if(courseReservationEntity is not null)
            {
                // update status
                courseReservationEntity.CourseReservationStatusId = 2;
                courseReservationEntity.LastModifiedDate = DateTime.Now;
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }
    }
}
