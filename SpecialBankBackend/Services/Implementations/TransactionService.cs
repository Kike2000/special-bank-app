using SpecialBankAPI.Models;
using SpecialBankAPI.Services.Interfaces;

namespace SpecialBankAPI.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        public Response CreateNewTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Response FindTransactionByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, int TransactionPin)
        {
            throw new NotImplementedException();
        }

        public Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            throw new NotImplementedException();
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, int TransactionPin)
        {
            throw new NotImplementedException();
        }
    }
}
