using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class CoursePackageReservationRepository : ICoursePackageReservationRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public CoursePackageReservationRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CoursePackageReservationModel> CreateAsync(CoursePackageReservation courseReservation)
        {
            await _context.CoursePackageReservations.AddAsync(courseReservation);
            bool isSucess = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!isSucess) return null;

            return _mapper.Map<CoursePackageReservationModel>(courseReservation);
        }

        public async Task<IEnumerable<CoursePackageReservationModel>> GetAllByCourseIdAsync(Guid courseId)
        {
            var courseReservationEntities = await _context.CoursePackageReservations
                .Select(x => new CoursePackageReservation { 
                    CoursePackageReservationId = x.CoursePackageReservationId,
                    CoursePackageId = x.CoursePackageId,
                    CreateDate = x.CreateDate,
                    CoursePackage = x.CoursePackage
                })
                .Where(x => x.CoursePackage.CourseId == courseId.ToString())
                .ToListAsync();
            return _mapper.Map<IEnumerable<CoursePackageReservationModel>>(courseReservationEntities);
        }

        public async Task<IEnumerable<MemberModel>> GetAllMemberInCourseAsync(Guid courseId)
        {
            var members = await _context.CoursePackageReservations.Where(x => 
                x.CoursePackage.CourseId == courseId.ToString())
                    .Select(x => x.Member)
                    .ToListAsync();

            return _mapper.Map<IEnumerable<MemberModel>>(members);
        }

        public async Task<CoursePackageReservationModel> GetByMemberAsync(Guid memberId)
        {
            var courseReservationEntity = await _context.CoursePackageReservations.Where(x => x.MemberId == memberId.ToString())
                                                                           .Select(x => new CoursePackageReservation { 
                                                                                CoursePackageReservationId = x.CoursePackageReservationId,
                                                                                CoursePackage = x.CoursePackage,
                                                                                StaffId = x.StaffId,
                                                                                CoursePackageId = x.CoursePackageId,
                                                                                CreateDate = x.CreateDate,
                                                                                MemberId = x.MemberId,
                                                                                PaymentTypeId = x.PaymentTypeId,
                                                                                ReservationStatusId = x.ReservationStatusId
                                                                           })
                                                                           .FirstOrDefaultAsync();
            return _mapper.Map<CoursePackageReservationModel>(courseReservationEntity);
        }

        public async Task<int> GetTotalMemberByMentorIdAsync(Guid mentorId)
        {
            var courseReservationEntities = await _context.CoursePackageReservations.Where(x => x.StaffId == mentorId.ToString())
                                                                             .ToListAsync();
            return courseReservationEntities.Count;
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid id, double paymentAmmount)
        {
            var courseReservationEntity = await _context.CoursePackageReservations.Where(x => x.CoursePackageReservationId == id.ToString())
                                                                           .FirstOrDefaultAsync();

            if(courseReservationEntity is not null)
            {
                // update status
                courseReservationEntity.ReservationStatusId = 2;
                // update payment ammount
                courseReservationEntity.PaymentAmmount = paymentAmmount;
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }
    }
}
