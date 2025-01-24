using SpecialBankAPI.Models;

namespace SpecialBankAPI.Services.Interfaces
{
    public interface ITransactionService
    {
        Response CreateNewTransaction(Transaction transaction);
        Response FindTransactionByDate(DateTime date);
        Response MakeDeposit(string AccountNumber, decimal Amount, int TransactionPin);
        Response MakeWithdrawal(string AccountNumber, decimal Amount, int TransactionPin);
        Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin);
    }
}
