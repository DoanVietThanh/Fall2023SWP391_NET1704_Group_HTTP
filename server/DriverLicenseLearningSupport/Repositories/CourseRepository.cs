using Amazon.S3.Model.Internal.MarshallTransformations;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DriverLicenseLearningSupport.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public CourseRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<CourseModel> CreateAsync(Course course)
        {
            // add course 
            await _context.Courses.AddAsync(course);
            // save changes
            bool isSucess = await _context.SaveChangesAsync() > 0 ? true : false;

            // failed <- cause error
            if (!isSucess) return null;

            // decode text editor
            course.CourseDesc = WebUtility.HtmlDecode(course.CourseDesc);
            return _mapper.Map<CourseModel>(course);
        }
        public async Task<CourseModel> GetAsync(Guid id)
        {
            // get course by id 
            var course = await _context.Courses.Where(x => x.CourseId == id.ToString() && x.IsActive == true)
                                                // foreach -> new course -> get list curriculum
                                                .Select(x => new Course
                                                {
                                                    CourseId = x.CourseId,
                                                    CourseDesc = x.CourseDesc,
                                                    CourseTitle = x.CourseTitle,
                                                    Cost = x.Cost,
                                                    IsActive = x.IsActive,
                                                    TotalSession = x.TotalSession,
                                                    Curricula = x.Curricula.Select(c => new Curriculum
                                                    {
                                                        CurriculumId = c.CurriculumId,
                                                        CurriculumDesc = c.CurriculumDesc,
                                                        CurriculumDetail = c.CurriculumDetail
                                                    }).ToList()
                                                }).FirstOrDefaultAsync();

            if (course is not null)
            {
                // decode text editor
                course.CourseDesc = WebUtility.HtmlDecode(course.CourseDesc);
                // response model
                return _mapper.Map<CourseModel>(course);
            }
            return null;
        }
        public async Task<IEnumerable<CourseModel>> GetAllAsync()
        {
            var courses = await _context.Courses.Where(x => x.IsActive == true).ToListAsync();
            return _mapper.Map<IEnumerable<CourseModel>>(courses);
        }
        public async Task<bool> AddCurriculumAsync(Guid courseId, int curriculumId) 
        {
            var courseEntity = await _context.Courses.Where(x => x.CourseId == courseId.ToString())
                                                     .FirstOrDefaultAsync();
            if(courseEntity is not null) 
            {
                var curriculumEntity = await _context.Curricula.Where(x => x.CurriculumId == curriculumId)
                                                               .FirstOrDefaultAsync();
                courseEntity.Curricula.Add(curriculumEntity);
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }
        public async Task<bool> UpdateAsync(Guid id, CourseModel course)
        {
            var courseEntity = await _context.Courses.Where(x => x.CourseId == id.ToString())
                                                     .FirstOrDefaultAsync();
            if (courseEntity is not null) 
            {
                // update fields
                courseEntity.CourseTitle = course.CourseTitle;
                courseEntity.CourseDesc = course.CourseDesc;
                courseEntity.CourseDesc = course.CourseDesc;
                courseEntity.TotalSession = course.TotalSession;

                // save changes
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }

            return false;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            // get course by id
            var courseEntity = await _context.Courses.Where(x => x.CourseId == id.ToString())
                                                     .FirstOrDefaultAsync();

            if(courseEntity is not null) 
            {
                // remove <- change status
                _context.Courses.Remove(courseEntity);
                //courseEntity.IsActive = false;
                // save changes
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }
        public async Task<bool> HideCourseAsync(Guid id)
        {
            // get course by id
            var courseEntity = await _context.Courses.Where(x => x.CourseId == id.ToString())
                                                     .FirstOrDefaultAsync();

            if (courseEntity is not null)
            {
                // hide <- change status
                courseEntity.IsActive = false;
                // save changes
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }
        public async Task<IEnumerable<CourseModel>> GetAllHiddenCourseAsync()
        {
            var courses = await _context.Courses.Where(x => x.IsActive == false).ToListAsync();
            return _mapper.Map<IEnumerable<CourseModel>>(courses);
        }

    }
}
