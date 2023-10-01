using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;

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
    }
}
