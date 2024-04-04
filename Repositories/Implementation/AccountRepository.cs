using GeoQuest.DTOs;
using GeoQuest.Enums;
using GeoQuest.Models;
using GeoQuest.Utils;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace GeoQuest.Repositories.Implementation
{
    public class AccountRepository : IAccountRepository
    {

        private readonly GeoQuestContext _context;

        public AccountRepository(GeoQuestContext context)
        {
            _context = context;
        }



        public async Task ChangePassword(int accountId, string password)
        {
            string hash = null;
            string salt = null;

            var passwordHashingHandler = new PasswordHashingHandler(password);
            passwordHashingHandler.CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt);

            hash = Convert.ToBase64String(passwordHash);
            salt = Convert.ToBase64String(passwordSalt);

            var account = await _context.Account.FirstOrDefaultAsync(acc => acc.Id == accountId);

            account!.Password = hash;
            account!.Salt = salt;

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateAccount(RegisterDto registerDto)
        {
            string password = null;
            string salt = null;

            if (registerDto.Password is not null)
            {
                var passwordHashingHandler = new PasswordHashingHandler(registerDto.Password);
                passwordHashingHandler.CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt);

                password = Convert.ToBase64String(passwordHash);
                salt = Convert.ToBase64String(passwordSalt);
            }


            Account newAccount = new Account
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Password = password,
                Salt = salt,
                Role = (int)registerDto.Role
            };

            _context.Add(newAccount);
            await _context.SaveChangesAsync();

            return await Task.FromResult(newAccount.Id);
        }

        public async Task<int?> FindByEmail(string email)
        {
            var account = await _context.Account.Where(acc => acc.Email == email).SingleOrDefaultAsync();

            if (account == null)
            {
                return null;
            }
            else
            {
                return account.Id;
            }
        }

        public async Task<Account?> GetAccount(int id)
        {
            return await _context.Account.FindAsync(id);
        }

        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _context.Account.FirstOrDefaultAsync(acc => acc.Email == email);
        }
    }
}
