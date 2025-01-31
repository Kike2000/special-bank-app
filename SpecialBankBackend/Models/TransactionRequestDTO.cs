using System.ComponentModel.DataAnnotations;

namespace SpecialBankAPI.Models
{
    public class TransactionRequestDTO
    {
        public decimal TransactionAmount { get; set; }
        public string TransactionSourceAccount { get; set; }
        public string TransactionDestinationAccount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
