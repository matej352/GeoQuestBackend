using GeoQuest.DTOs;
using GeoQuest.Enums;
using GeoQuest.Models;

namespace GeoQuest.Services
{
    public interface IAccountService
    {
        public Task<int> createAccount(RegisterDto registerDto);

        public Task<Account?> GetAccountByEmail(string email);

        public Task<AccountDto> GetAccount(int id);

        public Task<IEnumerable<Account>> GetAccounts(UserRole role, int subjectId);
    }
}
