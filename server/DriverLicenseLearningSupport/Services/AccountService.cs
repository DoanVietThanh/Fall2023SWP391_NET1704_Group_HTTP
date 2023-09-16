using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Services.impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Services
{
    public class AccountService : IAccountService
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public AccountService(DriverLicenseLearningSupportContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<AccountModel> CheckLoginAsync(string username, string password)
        {
            var accountEntity = await _context.Accounts.Where(x => x.Email == username
                                                                && x.Password == password)
                                                       .FirstOrDefaultAsync();
            if (accountEntity != null)
            {
                accountEntity.Role = 
                    _context.Roles.Where(x => x.RoleId == accountEntity.RoleId).FirstOrDefault();
            }
            return _mapper.Map<AccountModel>(accountEntity);
        }

        public async Task<bool> CreateAsync(AccountModel account)
        {
            var accountEntity = _mapper.Map<Account>(account);
            _context.Accounts.Add(accountEntity);
            var result = await _context.SaveChangesAsync();
            if (result > 0) return true;
            return false;
        }

        public async Task<AccountModel> FindByEmailAsync(string email)
        {
            var account = await _context.Accounts.Where(x => x.Email == email)
                                                 .FirstOrDefaultAsync();
            return _mapper.Map<AccountModel>(account);
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var account = await _context.Accounts.Where(x => x.Email == email)
                                                 .FirstOrDefaultAsync();
            account.Password = newPassword;
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}
