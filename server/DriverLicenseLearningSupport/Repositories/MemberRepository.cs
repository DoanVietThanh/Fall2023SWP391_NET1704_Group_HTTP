using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DriverLicenseLearningSupport.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public MemberRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberModel> CreateAsync(Member member)
        {
            // add async
            await _context.Members.AddAsync(member);
            // save changes
            var isSucess = await _context.SaveChangesAsync() > 0 ? true : false;

            if (!isSucess) return null; // update failed
            // map entity to model <- update success
            return _mapper.Map<MemberModel>(member);
        }
        public async Task<IEnumerable<MemberModel>> CreateRangeAsync(IEnumerable<Member> members)
        {
            // add range members
            await _context.Members.AddRangeAsync(members);
            // save changes
            var isSucess = await _context.SaveChangesAsync() > 0 ? true : false;
            // save failed
            if (!isSucess) return null;
            // save succesfully
            return _mapper.Map<IEnumerable<MemberModel>>(members);
        }
        public async Task<MemberModel> GetByEmailAsync(string email)
        {
            // get by email
            var memberEntity = await _context.Members.Where(x => x.Email.Equals(email) && x.IsActive == true)
                                                .Select(x => new Member
                                                {
                                                    MemberId = x.MemberId,
                                                    FirstName = x.FirstName,
                                                    LastName = x.LastName,
                                                    AvatarImage = x.AvatarImage,
                                                    Phone = x.Phone,
                                                    DateBirth = x.DateBirth,
                                                    Email = x.Email,
                                                    LicenseType = new LicenseType
                                                    {
                                                        LicenseTypeId = x.LicenseType.LicenseTypeId,
                                                        LicenseTypeDesc = x.LicenseType.LicenseTypeDesc
                                                    },
                                                    Address = new Address
                                                    {
                                                        AddressId = x.AddressId,
                                                        Street = x.Address.Street,
                                                        District = x.Address.District,
                                                        City = x.Address.City,
                                                        Zipcode = x.Address.Zipcode,
                                                    },
                                                    EmailNavigation = new Account
                                                    {
                                                        Role = new Role
                                                        {
                                                            RoleId = x.EmailNavigation.Role.RoleId,
                                                            Name = x.EmailNavigation.Role.Name
                                                        }
                                                    },
                                                    LicenseFormId = x.LicenseFormId
                                                    //LicenseForm = new LicenseRegisterForm { 
                                                    //    LicenseFormId = x.LicenseForm.LicenseFormId,
                                                    //    LicenseFormDesc = x.LicenseForm.LicenseFormDesc,
                                                    //    CreateDate = x.LicenseForm.CreateDate,
                                                    //    Image = x.LicenseForm.Image,
                                                    //    IdentityCardImage = x.LicenseForm.IdentityCardImage,
                                                    //    HealthCertificationImage = x.LicenseForm.HealthCertificationImage,
                                                    //    RegisterFormStatus = new LicenseRegisterFormStatus 
                                                    //    {
                                                    //        RegisterFormStatusId = x.LicenseForm.RegisterFormStatus.RegisterFormStatusId, 
                                                    //        RegisterFormStatusDesc = x.LicenseForm.RegisterFormStatus.RegisterFormStatusDesc 
                                                    //    }
                                                    //}
                                                }).FirstOrDefaultAsync();
            // map to model and return
            return _mapper.Map<MemberModel>(memberEntity);
        }
        public async Task<MemberModel> GetAsync(Guid id)
        {
            // get member by id
            var memberEntity = await _context.Members.Where(x => x.MemberId == id.ToString() && x.IsActive == true)
                                                     .FirstOrDefaultAsync();
            // map to model and return
            return _mapper.Map<MemberModel>(memberEntity);
        }
        public async Task<IEnumerable<MemberModel>> GetAllAsync()
        {
            // get all member and convert to IList
            var members = await _context.Members.Where(x => x.IsActive == true).ToListAsync();
            return _mapper.Map<IEnumerable<MemberModel>>(members);
        }
        public async Task<bool> UpdateAsync(Guid id, MemberModel member)
        {
            // find by id
            var memberEntity = await _context.Members.Where(x => x.MemberId == id.ToString())
                                                        .FirstOrDefaultAsync();
            // check exist
            if (memberEntity is null) return false;

            // update address
            var addressEntity = await _context.Addresses.Where(x => x.AddressId == memberEntity.AddressId)
                                                        .FirstOrDefaultAsync();
            addressEntity.Street = member.Address.Street;
            addressEntity.District = member.Address.District;
            addressEntity.City = member.Address.City;

            // update fields
            memberEntity.FirstName = member.FirstName;
            memberEntity.LastName = member.LastName;
            memberEntity.Phone = member.Phone;
            memberEntity.DateBirth = member.DateBirth;

            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        public async Task<IEnumerable<MemberModel>> GetAllAsyncByFilter(MemberFilter filters)
        {
            // building query with filters
            // members.Count = 0 <- just noted that "get all members", not execute yet
            var members = _context.Members.Where(x => x.IsActive == true).AsQueryable();

            // filter by name
            if (!String.IsNullOrEmpty(filters.Name))
            {
                members = members.Where(x => string.Concat(x.FirstName, " ", x.LastName).Contains(filters.Name));
            }

            // filter by birthdate
            if (!String.IsNullOrEmpty(filters.DateBirth))
            {
                // convert filter datetime to particular format
                var filterDateTime = DateTime.ParseExact(filters.DateBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                // find members with same birthdate
                members = members.Where(x => x.DateBirth == filterDateTime);
            }

            // filter by phone number
            if (!String.IsNullOrEmpty(filters.Phone))
            {
                members = members.Where(x => x.Phone.Equals(filters.Phone));
            }

            // filter by city
            if (!String.IsNullOrEmpty(filters.City))
            {
                members = members.Where(x => x.Address.City.Equals(filters.City));
            }

            return _mapper.Map<IEnumerable<MemberModel>>(await members.ToListAsync());
        }
        public async Task<MemberModel> GetByLicenseRegisterFormIdAsync(int licenseRegisterFormId)
        {
            var memberEntity = await _context.Members.Where(x => x.LicenseFormId == licenseRegisterFormId)
                                               .FirstOrDefaultAsync();
            return _mapper.Map<MemberModel>(memberEntity);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            // get member by id
            var member = await _context.Members.Where(x => x.MemberId.Equals(id.ToString()))
                                               .FirstOrDefaultAsync();
            // not found
            if (member is null) return false;
            //remove member
            _context.Members.Remove(member);
            // save changes and return 
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        public async Task<bool> HideMemberAsync(Guid id)
        {
            var member = await _context.Members.Where(x => x.MemberId == id.ToString())
                                         .FirstOrDefaultAsync();
            if (member is not null)
            {
                // hide <- change status
                member.IsActive = false;
                // save changes
                return await _context.SaveChangesAsync() > 0 ? true : false;
            }
            return false;
        }

    }
}
