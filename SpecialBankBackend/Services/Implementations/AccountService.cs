using Microsoft.EntityFrameworkCore;
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

        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(Pin)) throw new ArgumentNullException("Pin");
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Pin));
                for(int i =0; i< computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i]) return false;
                }
            }
            return true;
        }
        public async Task<Account> Authenticate(string AccountNumber, string Pin)
        {
            var account =await _specialBankDbContext.Account.Where(x=>x.AccountNumberGenerated == AccountNumber).SingleOrDefaultAsync();
            if (account == null)
                return null;
            if (!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
                return null;
            return account;
        }

        public async Task<Account> CreateAccount(Account account, string Pin, string ConfirmPin)
        {
            if (_specialBankDbContext.Account.Any(x => x.Email == account.Email)) throw new ApplicationException("An account already existing with this email!");
            if (!Pin.Equals(ConfirmPin)) throw new ArgumentException("The pins don't match", "Pin");

            byte[] pinHash, pinSalt;
            CreatePin(Pin, out pinHash, out pinSalt);
            account.PinHash= pinHash;
            account.PinSalt = pinSalt;
            var newAccount = _specialBankDbContext.Account.AddAsync(account);
            await _specialBankDbContext.SaveChangesAsync();
            return account;
        }

        public async void DeleteAccount(int Id)
        {
            var account = _specialBankDbContext.Account.Find(Id);
            if (account == null)
            {
                _specialBankDbContext.Account.Remove(account);
                await _specialBankDbContext.SaveChangesAsync();
            }
        }

        public Account GetAccountByAccountNumber(string AccountNumber)
        {
            var account = _specialBankDbContext.Account.FirstOrDefault(x => x.AccountNumberGenerated == AccountNumber);
            if (account == null) return null;
            return account;
        }

        public Account GetAccountById(int Id)
        {
            var account = _specialBankDbContext.Account.FirstOrDefault(x => x.Id == Id);
            if (account == null) return null;
            return account;
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _specialBankDbContext.Account.ToList();
        }
        private static void CreatePin(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        public void UpdateAccount(Account account, string Pin = null)
        {
            var accountUpdated = _specialBankDbContext.Account.Where(p=>p.Email == account.Email).SingleOrDefault();
            if (accountUpdated == null) throw new ApplicationException("Account doesn't exist");
            if (!string.IsNullOrWhiteSpace(account.Email))
            {
                if (_specialBankDbContext.Account.Any(x => x.Email == account.Email)) throw new ApplicationException($"This email {account.Email} already exists.");
                accountUpdated.Email = account.Email;
            }

            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                if (_specialBankDbContext.Account.Any(x => x.PhoneNumber == account.PhoneNumber)) throw new ApplicationException($"This phone number {account.Email} already exists.");
                accountUpdated.PhoneNumber = account.PhoneNumber;
            }
            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePin(Pin, out pinHash, out pinSalt);
                accountUpdated.PinHash = pinHash;
                accountUpdated.PinSalt = pinSalt;
            }
            accountUpdated.LastUpdatedDate = DateTime.UtcNow;
            _specialBankDbContext.Account.Update(accountUpdated);
            _specialBankDbContext.SaveChangesAsync();
        }
    }
}
