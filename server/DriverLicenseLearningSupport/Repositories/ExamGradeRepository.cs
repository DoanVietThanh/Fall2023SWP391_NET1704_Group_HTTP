using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace DriverLicenseLearningSupport.Repositories
{
    public class ExamGradeRepository : IExamGradeRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public ExamGradeRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper, IOptionsMonitor<AppSettings> monitor)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = monitor.CurrentValue;
        }
        public async Task<ExamGradeModel> CreateAsync(ExamGrade entity)
        {
            await _context.ExamGrades.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ExamGradeModel>(entity);
        }

        public async Task<bool> DeleteAsync(int examGradeId)
        {
            var examGrade = await _context.ExamGrades.FirstOrDefaultAsync(x => x.ExamGradeId == examGradeId);
            _context.ExamGrades.Remove(examGrade);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<List<ExamGradeModel>> GetAllByTheoryExamIdandEmailAsync(string Email, int TheoryExamId, DateTime StartedDate)
        {

            var ExamGrades = await _context.ExamGrades.Where(eg => eg.TheoryExamId == TheoryExamId &&
            eg.Email.Equals(Email)).ToListAsync();
            var formatedDate = StartedDate.ToString(_appSettings.DateTimeFormat);
            //StartedDate = DateTime.ParseExact(formatedDate, _appSettings.DateTimeFormat, CultureInfo.InvariantCulture);
            ExamGrades = ExamGrades.Where(x => x.StartDate.ToString(_appSettings.DateTimeFormat).Equals(formatedDate)).ToList();
            return _mapper.Map<List<ExamGradeModel>>(ExamGrades);
        }


    }
}
