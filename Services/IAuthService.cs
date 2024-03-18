using GeoQuest.DTOs;

namespace GeoQuest.Services
{
    public interface IAuthService
    {
        public Task<AccountDto> Login(LoginDto loginDto);
    }
}
