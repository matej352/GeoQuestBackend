using GeoQuest.DTOs;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Models;
using GeoQuest.Repositories;

namespace GeoQuest.Services.Implementation
{
    public class AccountService : IAccountService
    {

        private readonly UserContext _userContext;

        private readonly IAccountRepository _accountRepository;



        public AccountService(UserContext userContext, IAccountRepository accountRepository)
        {
            _userContext = userContext;
            _accountRepository = accountRepository;
        }

        public async Task<int> createAccount(RegisterDto registerDto)
        {

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                throw new Exception("Password and confirm password mismatch!");
            }

            var emailUnque = await isEmailUnique(registerDto.Email);
            if (!emailUnque)
            {
                throw new Exception($"Korisnički račun sa email adresom {registerDto.Email} već postoji.");
            }

            var newAccountId = await _accountRepository.CreateAccount(registerDto);


            return newAccountId;

        }


        public async Task<bool> isEmailUnique(string email)
        {
            var accountId = await _accountRepository.FindByEmail(email);

            if (accountId == null)
            {
                return true;
            }
            return false;
        }

        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _accountRepository.GetAccountByEmail(email);
        }


    }
}
