using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services.impl;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository,
            IRoleRepository roleRepository,
            IMapper mapper)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<AccountModel> CheckLoginAsync(string username, string password)
        {
            var account = await _accountRepository.FindByUsernameAndPasswordAsync(username, password);
            if (account != null)
            {
                var roleModel = await _roleRepository.FindByIdAsync(account.RoleId);
                account.Role = roleModel;
            }
            return account;
        }

        public async Task<bool> CreateAsync(AccountModel account)
        {
            var accountEntity = _mapper.Map<Account>(account);
            return await _accountRepository.CreateAsync(accountEntity);
        }

        public async Task<AccountModel> FindByEmailAsync(string email)
        {
            return await _accountRepository.FindByEmailAsync(email);
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            return await _accountRepository.ResetPasswordAsync(email, newPassword);
        }
    }
}
