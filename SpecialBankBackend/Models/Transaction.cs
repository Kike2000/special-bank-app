
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpecialBankAPI.Models
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string TransactionUniqueReference { get; set; }
        public decimal TransactionAmount { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public bool IsSuccesful => TransactionStatus.Equals(TransactionStatus.Success);
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
  
        public string TransactionParticulars { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
    public enum TransactionStatus
    {
        Success,
        Failed,
        Error
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }


}
