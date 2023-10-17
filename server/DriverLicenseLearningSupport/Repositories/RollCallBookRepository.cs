using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
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

        public async Task<RollCallBookModel> GetAsync(int id)
        {
            var rcbook = await _context.RollCallBooks.Where(x => x.RollCallBookId == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<RollCallBookModel>(rcbook);
        }

        public async Task<bool> UpdateAsync(int rcbId, RollCallBookModel rcbook)
        {
            var rcbookEntity = await _context.RollCallBooks.Where(x 
                    => x.RollCallBookId == rcbId)
                .FirstOrDefaultAsync();

            if(rcbookEntity is not null)
            {
                rcbookEntity.Comment = rcbook.Comment;
                rcbookEntity.IsAbsence = rcbook.IsAbsence;
                rcbookEntity.TotalHoursDriven = rcbook.TotalHoursDriven;
                rcbookEntity.TotalKmDriven = rcbook.TotalKmDriven;

                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
