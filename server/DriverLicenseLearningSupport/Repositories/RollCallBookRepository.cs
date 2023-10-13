using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class RollCallBookRepository : IRollCallBookRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public RollCallBookRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RollCallBookModel>> GetAllByMemberIdAsync(Guid memberId)
        {
            var rcbooks = await _context.RollCallBooks.Where(x => x.MemberId == memberId.ToString())
                                                      .ToListAsync();
            return _mapper.Map<IEnumerable<RollCallBookModel>>(rcbooks);
        }
    }
}
