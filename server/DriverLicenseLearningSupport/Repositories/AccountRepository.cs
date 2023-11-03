using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public AccountRepository(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(Account account)
        {
            await _context.AddAsync(account);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        public async Task<AccountModel> GetByEmailAsync(string email)
        {
            var accountEntity = await _context.Accounts.Where(x => x.Email == email)
                                                       .Select(x => new Account { 
                                                            Email = x.Email,
                                                            Password = x.Password,
                                                            IsActive = x.IsActive,
                                                            Role = new Role() 
                                                            {
                                                                RoleId = x.Role.RoleId,
                                                                Name = x.Role.Name
                                                            }
                                                       }).FirstOrDefaultAsync();
            return _mapper.Map<AccountModel>(accountEntity);
        }
        public async Task<AccountModel> GetByUsernameAndPasswordAsync(string username, string password)
        {
            var accountEntity = await _context.Accounts.Where(x => x.Email == username
                                                            && x.Password == password)
                                                       .Select(x => new Account
                                                       {
                                                           Email = x.Email,
                                                           Password = x.Password,
                                                           IsActive = x.IsActive,
                                                           Role = new Role()
                                                           {
                                                               RoleId = x.Role.RoleId,
                                                               Name = x.Role.Name
                                                           }
                                                       })
                                                       .FirstOrDefaultAsync();
            return _mapper.Map<AccountModel>(accountEntity);
        }
        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var account = await _context.Accounts.Where(x => x.Email == email)
                                                 .FirstOrDefaultAsync();
            if (account != null) account.Password = newPassword;
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        public async Task<bool> DeleteAsync(string email)
        {
            // get by email
            var account = await _context.Accounts.Where(x => x.Email == email)
                                                 .FirstOrDefaultAsync();
            // remove
            _context.Accounts.Remove(account);
            // save changes and return
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<AccountModel>> GetAllAsync()
        {
            var accounts = await _context.Accounts
                .Include(x => x.Role)
                .ToListAsync();
            return _mapper.Map<IEnumerable<AccountModel>>(accounts);
        }

        public async Task<bool> BanAccountAsync(string email)
        {
            var account = await _context.Accounts.Where(x => x.Email.Equals(email))
                                                 .FirstOrDefaultAsync();
            if(account is not null)
            {
                account.IsActive = false;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnBanAccountAsync(string email)
        {
            var account = await _context.Accounts.Where(x => x.Email.Equals(email))
                                                 .FirstOrDefaultAsync();
            if (account is not null)
            {
                account.IsActive = true;
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}