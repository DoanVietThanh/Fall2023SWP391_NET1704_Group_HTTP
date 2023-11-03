using Amazon.S3.Model.Internal.MarshallTransformations;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
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
        public async Task<CoursePackageModel> CreatePackageAsync(CoursePackage coursePackage)
        {
            // add package
            await _context.CoursePackages.AddAsync(coursePackage);
            // save changes
            bool isSucess = await _context.SaveChangesAsync() > 0 ? true : false;
            if (!isSucess) return null!; 
            return _mapper.Map<CoursePackageModel>(coursePackage);
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
                                                    IsActive = x.IsActive,
                                                    TotalHoursRequired = x.TotalHoursRequired,
                                                    TotalKmRequired = x.TotalKmRequired,
                                                    TotalMonth = x.TotalMonth,
                                                    StartDate = x.StartDate,
                                                    Curricula = x.Curricula.Select(c => new Curriculum
                                                    {
                                                        CurriculumId = c.CurriculumId,
                                                        CurriculumDesc = WebUtility.HtmlDecode(c.CurriculumDesc),
                                                        CurriculumDetail = WebUtility.HtmlDecode(c.CurriculumDetail)
                                                    }).ToList(),
                                                    Mentors = x.Mentors,
                                                    LicenseType = x.LicenseType,
                                                    LicenseTypeId = x.LicenseTypeId,
                                                    FeedBacks = x.FeedBacks.Select(x => new FeedBack {
                                                        FeedbackId = x.FeedbackId,
                                                        Content = x.Content,
                                                        RatingStar = x.RatingStar,
                                                        CreateDate = x.CreateDate,
                                                        Member = x.Member
                                                    }).ToList(),
                                                    CoursePackages = x.CoursePackages
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
        public async Task<CourseModel> GetHiddenCourseAsync(Guid id) 
        {
            var courseEntity = await _context.Courses.Where(x => x.CourseId == id.ToString()
                                                            && x.IsActive == false)
                                                        // foreach -> new course -> get list curriculum
                                                        .Select(x => new Course
                                                        {
                                                            CourseId = x.CourseId,
                                                            CourseDesc = x.CourseDesc,
                                                            CourseTitle = x.CourseTitle,
                                                            TotalKmRequired = x.TotalKmRequired,
                                                            TotalHoursRequired = x.TotalHoursRequired,
                                                            TotalMonth = x.TotalMonth,
                                                            IsActive = x.IsActive,
                                                            Curricula = x.Curricula.Select(c => new Curriculum
                                                            {
                                                                CurriculumId = c.CurriculumId,
                                                                CurriculumDesc = c.CurriculumDesc,
                                                                CurriculumDetail = c.CurriculumDetail
                                                            }).ToList()
                                                        }).FirstOrDefaultAsync();
            if (courseEntity is not null)
            {

                // decode text editor
                courseEntity.CourseDesc = WebUtility.HtmlDecode(courseEntity.CourseDesc);
                // response model
                return _mapper.Map<CourseModel>(courseEntity);
            }
            return null;
        }
        public async Task<CourseModel> GetByMentorIdAsync(Guid mentorId)
        {
            var courses = await _context.Courses.Where(x => x.IsActive == true)
                                                .Select(x => new Course
                                                {
                                                    CourseId = x.CourseId,
                                                    CourseDesc = x.CourseDesc,
                                                    CourseTitle = x.CourseTitle,
                                                    IsActive = x.IsActive,
                                                    TotalHoursRequired = x.TotalHoursRequired,
                                                    TotalKmRequired = x.TotalKmRequired,
                                                    TotalMonth = x.TotalMonth,
                                                    StartDate = x.StartDate,
                                                    Curricula = x.Curricula.Select(c => new Curriculum
                                                    {
                                                        CurriculumId = c.CurriculumId,
                                                        CurriculumDesc = c.CurriculumDesc,
                                                        CurriculumDetail = c.CurriculumDetail
                                                    }).ToList(),
                                                    Mentors = x.Mentors,
                                                    LicenseTypeId = x.LicenseTypeId
                                                }).ToListAsync();
            foreach (var c in courses)
            {
                var mentor = c.Mentors.Where(x => x.StaffId == mentorId.ToString()).FirstOrDefault();
                if (mentor != null)
                {
                    return _mapper.Map<CourseModel>(c);
                }
            }
            return null;
        }
        public async Task<CourseModel> GetByMentorIdAndCourseIdAsync(Guid mentorId, Guid courseId)
        {
            var courses = await _context.Courses.Where(x => x.IsActive == true && x.CourseId == courseId.ToString())
                                                .Select(x => new Course
                                                {
                                                    CourseId = x.CourseId,
                                                    CourseDesc = x.CourseDesc,
                                                    CourseTitle = x.CourseTitle,
                                                    TotalKmRequired = x.TotalKmRequired,
                                                    IsActive = x.IsActive,
                                                    TotalHoursRequired = x.TotalHoursRequired,
                                                    TotalMonth = x.TotalMonth,
                                                    StartDate = x.StartDate,
                                                    Curricula = x.Curricula.Select(c => new Curriculum
                                                    {
                                                        CurriculumId = c.CurriculumId,
                                                        CurriculumDesc = c.CurriculumDesc,
                                                        CurriculumDetail = c.CurriculumDetail
                                                    }).ToList(),
                                                    Mentors = x.Mentors,
                                                    LicenseTypeId = x.LicenseTypeId
                                                }).ToListAsync();
            foreach (var c in courses)
            {
                var mentor = c.Mentors.Where(x => x.StaffId == mentorId.ToString()).FirstOrDefault();
                if (mentor != null)
                {
                    return _mapper.Map<CourseModel>(c);
                }
            }
            return null;
        }
        public async Task<CoursePackageModel> GetPackageAsync(Guid packageId)
        {
            var coursePackageEntity = await _context.CoursePackages.Where(x => x.CoursePackageId == packageId.ToString())
                                                                   .Select(x => new CoursePackage { 
                                                                        CoursePackageId = x.CoursePackageId,
                                                                        CoursePackageDesc = x.CoursePackageDesc,
                                                                        Cost = x.Cost,
                                                                        AgeRequired = x.AgeRequired,
                                                                        Course = x.Course,
                                                                        // Optional
                                                                        SessionHour = x.SessionHour,
                                                                        TotalSession = x.TotalSession
                                                                    })
                                                                   .FirstOrDefaultAsync();
            return _mapper.Map<CoursePackageModel>(coursePackageEntity);
        }
        public async Task<IEnumerable<CourseModel>> GetAllAsync()
        {
            var courses = await _context.Courses.Select(x => new Course { 
                CourseId = x.CourseId,
                CourseTitle = x.CourseTitle,
                CourseDesc = x.CourseDesc,
                TotalMonth = x.TotalMonth,
                TotalKmRequired = x.TotalKmRequired,
                TotalHoursRequired = x.TotalHoursRequired,
                StartDate = x.StartDate,
                IsActive = x.IsActive,
                LicenseTypeId = x.LicenseTypeId,
                CoursePackages = x.CoursePackages
            }).ToListAsync();
            return _mapper.Map<IEnumerable<CourseModel>>(courses);
        }
        public async Task<IEnumerable<CourseModel>> GetAllMentorCourseAsync(Guid mentorId)
        {
            var courses = await _context.Courses.Where(x => x.IsActive == true)
                                                .Select(x => new Course { 
                                                    CourseId = x.CourseId,
                                                    CourseTitle = x.CourseTitle,
                                                    CourseDesc = x.CourseDesc,
                                                    TotalMonth = x.TotalMonth,
                                                    StartDate = x.StartDate,
                                                    IsActive = x.IsActive,
                                                    LicenseTypeId = x.LicenseTypeId,
                                                    Mentors = x.Mentors.Where(x => x.StaffId == mentorId.ToString()).ToList(),
                                                }).ToListAsync();

            var mentorCourses = courses.Where(x => x.Mentors.Count > 0).ToList();
            
            foreach(var c in mentorCourses)
            {
                c.Mentors = null!;
            }

            return _mapper.Map<IEnumerable<CourseModel>>(mentorCourses);
        }
        public async Task<IEnumerable<CourseModel>> GetAllHiddenCourseAsync()
        {
            var courses = await _context.Courses.Where(x => x.IsActive == false).ToListAsync();
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
        public async Task<bool> AddMentorAsync(Guid courseId, Guid mentorId)
        {
            var courseEntity = await _context.Courses.Where(x => x.CourseId == courseId.ToString())
                                                     .FirstOrDefaultAsync();
            if (courseEntity is not null)
            {
                var mentorEntity = await _context.Staffs.Where(x => x.StaffId == mentorId.ToString())
                                                               .FirstOrDefaultAsync();
                courseEntity.Mentors.Add(mentorEntity);
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }
        public async Task<bool> UpdateAsync(Guid id, Course course)
        {
            var courseEntity = await _context.Courses.Where(x => x.CourseId == id.ToString())
                                                     .FirstOrDefaultAsync();
            if (courseEntity is not null) 
            {
                // update fields
                courseEntity.CourseTitle = course.CourseTitle;
                courseEntity.CourseDesc = course.CourseDesc;
                courseEntity.TotalHoursRequired = course.TotalHoursRequired;
                courseEntity.TotalKmRequired = course.TotalKmRequired;

                // save changes
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }

            return false;
        }
        public async Task<bool> UpdateCourseCurriculumAsync(Guid courseId, Curriculum curriculum)
        {
            // get course by id
            var course = await _context.Courses.Where(x => x.CourseId == courseId.ToString() && x.IsActive == false)
                                                // foreach -> new course -> get list curriculum
                                                .Select(x => new Course
                                                {
                                                    CourseId = x.CourseId,
                                                    CourseDesc = x.CourseDesc,
                                                    CourseTitle = x.CourseTitle,
                                                    IsActive = x.IsActive,
                                                    Curricula = x.Curricula.Select(c => new Curriculum
                                                    {
                                                        CurriculumId = c.CurriculumId,
                                                        CurriculumDesc = c.CurriculumDesc,
                                                        CurriculumDetail = c.CurriculumDetail
                                                    }).ToList()
                                                }).FirstOrDefaultAsync();
            var curriculumEntity = course.Curricula.Where(x => x.CurriculumId == curriculum.CurriculumId)
                                              .FirstOrDefault();
            var courseCurri = await _context.Curricula.Where(x => x.CurriculumId == curriculumEntity.CurriculumId)
                                                      .FirstOrDefaultAsync();     
            // not found curriculum in course
            if (courseCurri is null) return false;

            // update curriculum
            courseCurri.CurriculumDesc = curriculum.CurriculumDesc;
            courseCurri.CurriculumDetail = curriculum.CurriculumDetail;

            // save changes
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            // get course by id
            var courseEntity = await _context.Courses.Where(x => x.CourseId == id.ToString() 
                                                        && x.IsActive == false)
                                                     .Include(x => x.Mentors)
                                                     .Include(x => x.CoursePackages)
                                                     .Include(x => x.Curricula)
                                                     .Include(x => x.FeedBacks)
                                                     .Include(x => x.WeekdaySchedules)
                                                     .FirstOrDefaultAsync();

            if(courseEntity.CoursePackages.Count > 0)
            {
                foreach (var cp in courseEntity.CoursePackages)
                {
                    var reservation = await _context.CoursePackageReservations.Where(x => x.CoursePackageId
                        == cp.CoursePackageId).Include(x => x.CoursePackage).ToListAsync();

                    _context.CoursePackageReservations.RemoveRange(reservation);
                }
                await _context.SaveChangesAsync();
            }

            if(courseEntity.WeekdaySchedules.Count > 0)
            {
                foreach (var ws in courseEntity.WeekdaySchedules)
                {
                    // get all weekday teaching schedule 
                    var schedule = await _context.WeekdaySchedules.Where(x => x.WeekdayScheduleId == ws.WeekdayScheduleId)
                        .Include(x => x.TeachingSchedules)
                        .FirstOrDefaultAsync();

                    // get all rollcall book for each teaching schedule
                    foreach (var ts in schedule.TeachingSchedules)
                    {
                        var teachingSchedule = await _context.TeachingSchedules.Where(x => x.TeachingScheduleId
                            == ts.TeachingScheduleId)
                            .Include(x => x.RollCallBooks)
                            .FirstOrDefaultAsync();

                        // remove teaching schedule
                        _context.TeachingSchedules.Remove(teachingSchedule);
                    }
                    // save changes remove teaching schedules
                    await _context.SaveChangesAsync();
                }
            }

            if (courseEntity is not null) 
            {
                courseEntity.WeekdaySchedules.Clear();
                courseEntity.CoursePackages.Clear();
                courseEntity.Mentors.Clear();
                courseEntity.Curricula.Clear();
                courseEntity.FeedBacks.Clear();
                // remove <- change status
                _context.Courses.Remove(courseEntity);
                //courseEntity.IsActive = false;
                // save changes
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }
        public async Task<bool> DeletePackageAsync(Guid id)
        {
            var coursePackage = await _context.CoursePackages.Where(x => x.CoursePackageId == id.ToString())
                .Include(x => x.TeachingSchedules)
                .FirstOrDefaultAsync();   

            if (coursePackage is null) return false;

            if (coursePackage.TeachingSchedules.Count() > 0) return false;

            //remove course package
            _context.CoursePackages.Remove(coursePackage);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> HideCourseAsync(Guid id)
        {
            // get course by id
            var courseEntity = await _context.Courses.Where(x => x.CourseId == id.ToString())
                                                     .FirstOrDefaultAsync();

            if (courseEntity is not null)
            {
                if (courseEntity.IsActive is false) return true;
                // hide <- change status
                courseEntity.IsActive = false;
                // save changes
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }
        public async Task<bool> UnhideAsync(Guid id)
        {
            var courseEntity = await _context.Courses.Where(x => x.CourseId == id.ToString())
                                                     .FirstOrDefaultAsync();
            if(courseEntity is not null) 
            {
                // unhide <- change status
                courseEntity.IsActive = true;
                // save changes and return
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            // save change failed
            return false;
        }
        public async Task<bool> UpdatePackageAsync(Guid packageId, CoursePackage package)
        {
            var packageEntity = await _context.CoursePackages.Where(x => x.CoursePackageId == packageId.ToString())
                                                             .FirstOrDefaultAsync();
            if (packageEntity is not null)
            {
                packageEntity.CoursePackageDesc = package.CoursePackageDesc;
                packageEntity.Cost = package.Cost;
                packageEntity.TotalSession = package.TotalSession;
                packageEntity.SessionHour = package.SessionHour;
                packageEntity.AgeRequired = package.AgeRequired;

                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }

    }
}
