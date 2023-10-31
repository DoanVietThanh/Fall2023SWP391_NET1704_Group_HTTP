using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

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

        public async Task<bool> ApproveCancelAsync(int rcbId)
        {
            var rcbook = await _context.RollCallBooks.Where(x => x.RollCallBookId == rcbId)
                .FirstOrDefaultAsync();

            if(rcbook is not null)
            {
                _context.RollCallBooks.Remove(rcbook);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> DenyCancelSchedule(int rcbId)
        {
            var rcbEntity = await _context.RollCallBooks.Where(x => x.RollCallBookId == rcbId)
                                                        .FirstOrDefaultAsync();

            if(rcbEntity is not null)
            {
                rcbEntity.IsActive = true;
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<RollCallBookModel>> GetAllByMemberIdAsync(Guid memberId)
        {
            var rcbooks = await _context.RollCallBooks.Where(x => x.MemberId == memberId.ToString())
                                                      .ToListAsync();
            return _mapper.Map<IEnumerable<RollCallBookModel>>(rcbooks);
        }

        public async Task<IEnumerable<RollCallBookModel>> GetAllInActiveRollCallBookAsync()
        {
            var rcbooks = await _context.RollCallBooks.Where(x => x.IsActive == false 
                                                        && x.IsAbsence == true)
                                                      .Select(x => new RollCallBook { 
                                                        RollCallBookId = x.RollCallBookId,
                                                        IsActive = x.IsActive,
                                                        IsAbsence = x.IsAbsence,
                                                        Comment = x.Comment,
                                                        TeachingScheduleId = x.TeachingScheduleId,
                                                        TotalHoursDriven = x.TotalHoursDriven,
                                                        TotalKmDriven = x.TotalKmDriven,
                                                        CancelMessage = x.CancelMessage,
                                                        Member = x.Member,
                                                        TeachingSchedule = new TeachingSchedule()
                                                        {
                                                            TeachingScheduleId = x.TeachingScheduleId,
                                                            TeachingDate = x.TeachingSchedule.TeachingDate,
                                                            StaffId = x.TeachingSchedule.StaffId,
                                                            SlotId = x.TeachingSchedule.SlotId,
                                                            VehicleId = x.TeachingSchedule.VehicleId,
                                                            WeekdayScheduleId = x.TeachingSchedule.WeekdayScheduleId,
                                                            CoursePackageId = x.TeachingSchedule.CoursePackageId,
                                                            IsActive = x.TeachingSchedule.IsActive,
                                                            Staff = x.TeachingSchedule.Staff,
                                                            CoursePackage = x.TeachingSchedule.CoursePackage
                                                        }
                                                      })
                                                      .ToListAsync();
            return _mapper.Map<IEnumerable<RollCallBookModel>>(rcbooks);
        }

        public async Task<RollCallBookModel> GetAsync(int id)
        {
            var rcbook = await _context.RollCallBooks.Where(x => x.RollCallBookId == id)
                .Select(x => new RollCallBook { 
                    RollCallBookId = x.RollCallBookId,
                    IsAbsence = x.IsAbsence,
                    Comment = x.Comment,
                    MemberId = x.MemberId,
                    Member = x.Member,
                    TotalHoursDriven = x.TotalHoursDriven,
                    TotalKmDriven = x.TotalKmDriven,
                    TeachingScheduleId = x.TeachingScheduleId
                })
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
                // get total hours driven
                var totalHour = ((rcbookEntity.TotalHoursDriven == null) ? 0 
                    : rcbookEntity.TotalHoursDriven) + rcbook.TotalHoursDriven;
                // get total km driven
                var totalKm = ((rcbookEntity.TotalKmDriven == null) ? 0 
                    : rcbookEntity.TotalKmDriven) + rcbook.TotalKmDriven;


                rcbookEntity.Comment = rcbook.Comment;
                rcbookEntity.IsAbsence = rcbook.IsAbsence;
                rcbookEntity.TotalHoursDriven = totalHour;
                rcbookEntity.TotalKmDriven = totalKm;

                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> UpdateInActiveStatusAsync(int rcbId, string cancelMessage)
        {
            var rcbEntity = await _context.RollCallBooks.Where(x => x.RollCallBookId == rcbId)
                                                        .FirstOrDefaultAsync();

            if(rcbEntity is not null &&
               rcbEntity.IsAbsence == true)
            {
                rcbEntity.IsActive = false;
                rcbEntity.CancelMessage = cancelMessage;

                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
