using SpecialBankAPI.Data;
using SpecialBankAPI.Models;
using SpecialBankAPI.Services.Interfaces;
using System.Text;

namespace SpecialBankAPI.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly SpecialBankDbContext _specialBankDbContext;
        public AccountService(SpecialBankDbContext specialBankDbContext)
        {
            _specialBankDbContext = specialBankDbContext;
        } 
        public Account Authenticate(string AccountNumber, string Pin)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> CreateAccount(Account account, string Pin, string ConfirmPin)
        {
            if (_specialBankDbContext.Account.Any(x => x.Email == account.Email)) throw new ApplicationException("An account already existing with this email!");
            if (!Pin.Equals(ConfirmPin)) throw new ArgumentException("The pins don't match", "Pin");

            byte[] pinHash, pinSalt;
            CreatePin(Pin, out pinHash, out pinSalt);
            var newAccount = _specialBankDbContext.Account.AddAsync(new Models.Account
            {
                FirstName = "Pedro",
                LastName = "Carrillo",
                Email = "pedro@gmail.com",
                PinHash = pinHash,
                PinSalt = pinSalt,
                PhoneNumber = "1234567890",
                CreatedDate = DateTime.UtcNow,
            });
            await _specialBankDbContext.SaveChangesAsync();
            return account;
        }

        public void DeleteAccount(int Id)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountByAccountNumber(int AccountNumber)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountById(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            throw new NotImplementedException();
        }
        private static void CreatePin(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }
    }
}
