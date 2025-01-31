using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SpecialBankAPI.Data;
using SpecialBankAPI.Models;
using SpecialBankAPI.Services.Interfaces;
using SpecialBankAPI.Utils;
using System.Text.Json.Serialization;

namespace SpecialBankAPI.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private SpecialBankDbContext _specialBankDbContext { get; set; }
        ILogger<TransactionService> _logger;
        private AppSettings _settings;
        public static string _specialBankSettlementAccount { get; set; }
        private readonly IAccountService _accountService;
        public TransactionService(SpecialBankDbContext specialBankDbContext, ILogger<TransactionService> logger, IOptions<AppSettings> settings, IAccountService accountService)
        {
            _specialBankDbContext = specialBankDbContext;
            _logger = logger;
            _settings = settings.Value;
            _specialBankSettlementAccount = _settings.SpecialBankSettlementAccount;
            _accountService = accountService;
        }

        public Response CreateNewTransaction(Transaction transaction)
        {
            Response response = new Response();
            _specialBankDbContext.Transaction.Add(transaction);
            _specialBankDbContext.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction has been created.";
            response.Data = null;
            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction =  _specialBankDbContext.Transaction.Where(x=>x.TransactionDate ==date);
            _specialBankDbContext.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction found";
            response.Data = null;
            return response;
        }

        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //make deposit 
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid credentials");

            try
            {
                sourceAccount = _accountService.GetAccountByAccountNumber(_specialBankSettlementAccount);
                destinationAccount = _accountService.GetAccountByAccountNumber(AccountNumber);  
                
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if((_specialBankDbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_specialBankDbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified)){
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Succesful Deposit";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TransactionStatus.Failed;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Succesful Deposit";
                    response.Data = null;
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error has occurred: {ex.Message}");
            }
            transaction.TransactionType = TransactionType.Deposit;
            transaction.TransactionSourceAccount = _specialBankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"New transaction from source: {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} to destination account: " +
                $"{JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} on date: {transaction.TransactionDate} for amount: {JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $"transaction type: {transaction.TransactionType} and transaction status: {transaction.TransactionStatus}";
            _specialBankDbContext.Transaction.Add( transaction );
            _specialBankDbContext.SaveChanges();
            return response;
        }

        public Response MakeFundsTransfer(string FromAccount, string ToAccount, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(FromAccount, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid credentials");

            try
            {
                sourceAccount = _accountService.GetAccountByAccountNumber(FromAccount);
                destinationAccount = _accountService.GetAccountByAccountNumber(ToAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_specialBankDbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_specialBankDbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Succesful Deposit";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TransactionStatus.Failed;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Succesful Deposit";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred: {ex.Message}");
            }
            transaction.TransactionType = TransactionType.Transfer;
            transaction.TransactionSourceAccount = FromAccount;
            transaction.TransactionDestinationAccount = ToAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"New transaction from source: {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} to destination account: " +
                $"{JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} on date: {transaction.TransactionDate} for amount: {JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $"transaction type: {transaction.TransactionType} and transaction status: {transaction.TransactionStatus}";
            _specialBankDbContext.Transaction.Add(transaction);
            _specialBankDbContext.SaveChanges();
            return response;
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            //make deposit 
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);
            if (authUser == null) throw new ApplicationException("Invalid credentials");

            try
            {
                sourceAccount = _accountService.GetAccountByAccountNumber(AccountNumber);
                destinationAccount = _accountService.GetAccountByAccountNumber(_specialBankSettlementAccount);

                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                if ((_specialBankDbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_specialBankDbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    transaction.TransactionStatus = TransactionStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Succesful Deposit";
                    response.Data = null;
                }
                else
                {
                    transaction.TransactionStatus = TransactionStatus.Failed;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Succesful Deposit";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error has occurred: {ex.Message}");
            }
            transaction.TransactionType = TransactionType.Withdrawal;
            transaction.TransactionSourceAccount = AccountNumber;
            transaction.TransactionDestinationAccount = _specialBankSettlementAccount ;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"New transaction from source: {JsonConvert.SerializeObject(transaction.TransactionSourceAccount)} to destination account: " +
                $"{JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)} on date: {transaction.TransactionDate} for amount: {JsonConvert.SerializeObject(transaction.TransactionAmount)} " +
                $"transaction type: {transaction.TransactionType}  and transaction status:  {transaction.TransactionStatus}";
            _specialBankDbContext.Transaction.Add(transaction);
            _specialBankDbContext.SaveChanges();
            return response;
        }
    }
}
 