using SpecialBankAPI.Models;

namespace SpecialBankAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Account Authenticate(string AccountNumber, string Pin);
        IEnumerable<Account> GetAllAccounts();
        Task<Account> CreateAccount(Account account, string Pin, string ConfirmPin);
        void DeleteAccount(int Id);
        Account GetAccountById(int Id);
        Account GetAccountByAccountNumber(int AccountNumber);
    }
}
