using GeoQuest.DTOs;
using GeoQuest.Models;

namespace GeoQuest.Services
{
    public interface IAccountService
    {
        public Task<int> createAccount(RegisterDto registerDto);

        public Task<Account?> GetAccountByEmail(string email);
    }
}
