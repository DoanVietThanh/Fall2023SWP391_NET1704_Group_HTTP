using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;

namespace DriverLicenseLearningSupport.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public StaffRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(Staff staff)
        {
            _context.Staffs.Add(staff);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<StaffModel> GetByEmailAsync(string email)
        {
            var staffEntity = await _context.Staffs.Where(x => x.Email == email)
                                                   .Select(x => new Staff
                                                   {
                                                       StaffId = x.StaffId,
                                                       FirstName = x.FirstName,
                                                       LastName = x.LastName,
                                                       DateBirth = x.DateBirth,
                                                       Email = x.Email,
                                                       AddressId = x.AddressId,
                                                       JobTitleId = x.JobTitleId,
                                                       LicenseTypeId = x.LicenseTypeId,
                                                       AvatarImage = x.AvatarImage,
                                                       Phone = x.Phone,
                                                       Address = x.Address,
                                                       LicenseType = x.LicenseType,
                                                       JobTitle = x.JobTitle,
                                                       EmailNavigation = new Account
                                                       {
                                                           Role = new Role
                                                           {
                                                               RoleId = x.EmailNavigation.Role.RoleId,
                                                               Name = x.EmailNavigation.Role.Name
                                                           }
                                                       }
                                                   }).FirstOrDefaultAsync();
            return _mapper.Map<StaffModel>(staffEntity);
        }

        public async Task<StaffModel> GetAsync(Guid id)
        {
            var staffEntity = await _context.Staffs.Where(x => x.StaffId == id.ToString())
                                                      .Select(x => new Staff
                                                      {
                                                          StaffId = x.StaffId,
                                                          FirstName = x.FirstName,
                                                          LastName = x.LastName,
                                                          Phone = x.Phone,
                                                          DateBirth = x.DateBirth,
                                                          AvatarImage = x.AvatarImage,
                                                          Email = x.Email,
                                                          AddressId = x.AddressId,
                                                          JobTitleId = x.JobTitleId,
                                                          LicenseTypeId = x.LicenseTypeId,
                                                          Address = x.Address,
                                                          JobTitle = x.JobTitle,
                                                          LicenseType = x.LicenseType,
                                                          SelfDescription = WebUtility.UrlDecode(x.SelfDescription),
                                                          EmailNavigation = new Account
                                                          {
                                                              Role = x.EmailNavigation.Role
                                                          },
                                                          FeedBacks = x.FeedBacks,
                                                          Courses = x.Courses
                                                      })
                                                      .FirstOrDefaultAsync();
            return _mapper.Map<StaffModel>(staffEntity);
        }

        public async Task<StaffModel> GetMentorAsync(Guid id)
        {
            var mentorEntity = await _context.Staffs.Where(x => x.StaffId == id.ToString())
                                              .Select(x => new Staff
                                              {
                                                  StaffId = x.StaffId,
                                                  FirstName = x.FirstName,
                                                  LastName = x.LastName,
                                                  DateBirth = x.DateBirth,
                                                  Email = x.Email,
                                                  AddressId = x.AddressId,
                                                  JobTitleId = x.JobTitleId,
                                                  LicenseTypeId = x.LicenseTypeId,
                                                  AvatarImage = x.AvatarImage,
                                                  Phone = x.Phone,
                                                  Address = x.Address,
                                                  LicenseType = x.LicenseType,
                                                  JobTitle = x.JobTitle,
                                                  EmailNavigation = new Account
                                                  {
                                                      Email = x.Email,
                                                      Role = x.EmailNavigation.Role
                                                  }
                                              }).FirstOrDefaultAsync();
            if(mentorEntity is not null)
            {
                var isMentorRole = mentorEntity.EmailNavigation.Role.Name.Equals("Mentor");
                if (!isMentorRole) return null;
            }
            return _mapper.Map<StaffModel>(mentorEntity);
        }
        public async Task<IEnumerable<StaffModel>> GetAllMentorAsync()
        {
            var mentorEntities = await _context.Staffs
                .Select(x => new Staff
                {
                    StaffId = x.StaffId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Phone = x.Phone,
                    DateBirth = x.DateBirth,
                    AvatarImage = x.AvatarImage,
                    Email = x.Email,
                    AddressId = x.AddressId,
                    JobTitleId = x.JobTitleId,
                    LicenseTypeId = x.LicenseTypeId,
                    Address = x.Address,
                    JobTitle = x.JobTitle,
                    LicenseType = x.LicenseType,
                    EmailNavigation = new Account
                    {
                        Role = x.EmailNavigation.Role
                    }
                }).ToListAsync();
            mentorEntities = mentorEntities.Where(x => x.EmailNavigation.Role.Name.Equals("Mentor")).ToList();
            return _mapper.Map<IEnumerable<StaffModel>>(mentorEntities);
        }
        public async Task<IEnumerable<StaffModel>> GetAllAsync()
        {
            var staffEntities = await _context.Staffs.Select(x => new Staff
            {
                StaffId = x.StaffId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Phone = x.Phone,
                DateBirth = x.DateBirth,
                AvatarImage = x.AvatarImage,
                Email = x.Email,
                AddressId = x.AddressId,
                JobTitleId = x.JobTitleId,
                LicenseTypeId = x.LicenseTypeId,
                Address = x.Address,
                JobTitle = x.JobTitle,
                LicenseType = x.LicenseType,
                EmailNavigation = new Account
                {
                    Role = x.EmailNavigation.Role
                }
            }).ToListAsync();
            return _mapper.Map<IEnumerable<StaffModel>>(staffEntities);
        }

        public async Task<IEnumerable<StaffModel>> GetAllByFilterAsync(StaffFilter filters)
        {
            // building query
            var staffs = _context.Staffs.AsQueryable();

            if (!String.IsNullOrEmpty(filters.Name)) // filter by name
            {
                // get staffs whose name match filters name
                staffs = staffs.Where(x => string.Concat(x.FirstName, " ", x.LastName)
                               .Contains(filters.Name));
            }

            if (!String.IsNullOrEmpty(filters.DateBirth)) // filter by date of birth
            {
                // convert datetime to particular format
                var formatDate = DateTime.ParseExact(filters.DateBirth, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture);
                // get staffs whose birthdate is formatDate
                staffs = staffs.Where(x => x.DateBirth == formatDate);
            }

            if (!String.IsNullOrEmpty(filters.Phone)) // filter by phone number 
            { 
                staffs = staffs.Where(x => x.Phone ==  filters.Phone);  
            }

            if (!String.IsNullOrEmpty(filters.Street)) // filter by street
            {
                staffs = staffs.Where(x => x.Address.Street.Contains(filters.Street));
            }

            if (!String.IsNullOrEmpty(filters.City)) // filter by city
            { 
                staffs = staffs.Where(x => x.Address.City.Contains(filters.City));
            }

            if (!String.IsNullOrEmpty(filters.District)) // filter by district
            { 
                staffs = staffs.Where(x => x.Address.District.Contains(filters.District));
            }

            if (!String.IsNullOrEmpty(filters.LicenseTypeId.ToString())) // filter by license type
            {
                staffs = staffs.Where(x => x.LicenseTypeId == filters.LicenseTypeId);
            }

            if (!String.IsNullOrEmpty(filters.JobTitleId.ToString())) // filter by job title
            {
                staffs = staffs.Where(x => x.JobTitleId == filters.JobTitleId);
            }

            if (!String.IsNullOrEmpty(filters.RoleId.ToString())) // filter by role 
            { 
                staffs = staffs.Where(x => x.EmailNavigation.Role.RoleId == filters.RoleId);
            }

            // get relationship
            staffs = staffs.Select(x => new Staff
                    {
                        StaffId = x.StaffId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Phone = x.Phone,
                        DateBirth = x.DateBirth,
                        AvatarImage = x.AvatarImage,
                        Email = x.Email,
                        AddressId = x.AddressId,
                        JobTitleId = x.JobTitleId,
                        LicenseTypeId = x.LicenseTypeId,
                        Address = x.Address,
                        JobTitle = x.JobTitle,
                        LicenseType = x.LicenseType,
                        EmailNavigation = new Account
                        {
                            Role = x.EmailNavigation.Role
                        }
                    });

            // mapping model and return
            return _mapper.Map<IEnumerable<StaffModel>>(await staffs.ToListAsync());
        }

        public async Task<bool> UpdateAsync(Guid id, Staff staff)
        {
            // get staff by id
            var staffEntity = await _context.Staffs.Where(x => x.StaffId == id.ToString())
                                                   .FirstOrDefaultAsync();
            // not found
            if (staffEntity is null) return false;

            // update staff fields
            staffEntity.FirstName = staff.FirstName;
            staffEntity.LastName = staff.LastName;
            staffEntity.DateBirth = staff.DateBirth;
            staffEntity.Phone = staff.Phone;
            staffEntity.AvatarImage = staff.AvatarImage;
            staffEntity.Address.Street = staff.Address.Street;
            staffEntity.Address.District = staff.Address.District;
            staffEntity.Address.City = staff.Address.City;
            staffEntity.JobTitleId = staff.JobTitleId;
            staffEntity.LicenseTypeId = staff.LicenseTypeId;

            // save changes and return 
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> DeleteAsync(Guid id) 
        {
            // get staff by id
            var staffEntity = await _context.Staffs.Where(x => x.StaffId == id.ToString())
                                                   .FirstOrDefaultAsync();

            if(staffEntity is null) return false;

            // remove staff
            _context.Staffs.Remove(staffEntity);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

    }
}
