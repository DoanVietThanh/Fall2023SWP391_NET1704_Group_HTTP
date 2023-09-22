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
            var memberEntity = await _context.Members.Where(x => x.Email.Equals(email))
                                                     .FirstOrDefaultAsync();
            // map to model and return
            return _mapper.Map<MemberModel>(memberEntity);
        }

        public async Task<MemberModel> GetAsync(Guid id)
        {
            // get member by id
            var memberEntity = await _context.Members.Where(x => x.MemberId == id.ToString())
                                                     .FirstOrDefaultAsync();
            // map to model and return
            return _mapper.Map<MemberModel>(memberEntity);
        }

        public async Task<IEnumerable<MemberModel>> GetAllAsync()
        {
            // get all member and convert to IList
            var members = await _context.Members.ToListAsync();
            return _mapper.Map<IEnumerable<MemberModel>>(members);
        }
        public async Task<bool> UpdateAsync(Guid id, MemberModel member)
        {
            // find by id
            var memberEntity = await _context.Members.Where(x => x.MemberId == id.ToString())
                                                        .FirstOrDefaultAsync();
            // check exist
            if (memberEntity is null) return false;

            // update fields
            memberEntity.FirstName = member.FirstName;
            memberEntity.LastName = member.LastName;
            memberEntity.Email = member.Email;
            memberEntity.Phone = member.Phone;
            memberEntity.DateBirth = member.DateBirth;

            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<MemberModel>> GetAllAsyncByFilter(MemberFilter filters)
        {
            // building query with filters
            // members.Count = 0 <- just noted that "get all members", not execute yet
            var members = _context.Members.AsQueryable();

            // filter by name
            if (!String.IsNullOrEmpty(filters.Name))
            {
                members = members.Where(x => x.FirstName.Contains(filters.Name) 
                                || x.LastName.Contains(filters.Name));
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            // get member by id
            var member = await _context.Members.Where(x => x.MemberId.Equals(id.ToString()))
                                               .FirstOrDefaultAsync();
            // not found
            if(member is null) return false;
            //remove member
            _context.Members.Remove(member);
            // save changes and return 
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

    }   
}
