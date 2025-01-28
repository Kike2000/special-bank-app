using SpecialBankAPI.Models;

namespace SpecialBankAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Account Authenticate(string AccountNumber, string Pin);
        IEnumerable<Account> GetAllAccounts();
        Account CreateAccount(Account account, string Pin, string ConfirmPin);
        void DeleteAccount(int Id);
        void UpdateAccount(Account account, string Pin = null);
        Account GetAccountById(int Id);
        Account GetAccountByAccountNumber(string AccountNumber);
    }
}
