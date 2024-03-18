using GeoQuest.DTOs;
using GeoQuest.DTOs.Extensions;
using GeoQuest.Middlewares.UserContext;
using GeoQuest.Repositories;
using GeoQuest.Utils;

namespace GeoQuest.Services.Implementation
{
    public class AuthService : IAuthService
    {

        private readonly UserContext _userContext;

        private readonly IAuthRepository _authRepository;

        private readonly IAccountService _accountService;


        public AuthService(UserContext userContext, IAuthRepository authRepository, IAccountService accountService)
        {
            _userContext = userContext;
            _authRepository = authRepository;
            _accountService = accountService;
        }


        public async Task<AccountDto> Login(LoginDto loginDto)
        {
            var account = await _accountService.GetAccountByEmail(loginDto.Email);

            if (account is null)
            {
                throw new Exception("Neispravan email ili lozinka!");
            }
            var passwordHashingHandler = new PasswordHashingHandler(loginDto.Password);


            if (!passwordHashingHandler.VerifyPasswordHash(Convert.FromBase64String(account.Password), Convert.FromBase64String(account.Salt)))
            {
                throw new Exception("Neispravan email ili lozinka!");
            }

            return account.AsAccountDto();
        }


    }
}
