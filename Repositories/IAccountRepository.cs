using GeoQuest.DTOs;
using GeoQuest.Enums;
using GeoQuest.Models;
using Task = System.Threading.Tasks.Task;

namespace GeoQuest.Repositories
{
    public interface IAccountRepository
    {

        public Task<int> CreateAccount(RegisterDto registerDto);


        public Task<Account?> GetAccountByEmail(string email);

        public Task<int?> FindByEmail(string email);


        public Task<Account?> GetAccount(int id);

        public Task<IEnumerable<Account>> GetAccounts(UserRole role, int subjectId);

        public Task ChangePassword(int accountId, string password);
    }
}
