using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpecialBankAPI.Models
{
    [Table("Accounts")]
    public class Account
    {
        Random rdm = new Random();
        public Account()
        {
            AccountNumberGenerated = Convert.ToString((long) rdm.NextDouble() * 9_000_000_000L + 1_000_000_000L);
            AccountName = $"{FirstName} {LastName}";
        }

        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }
        //Account Transaction PIN
        public byte[] PinHash { get; set; }
        public byte[] PinSalt { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }


    }
    public enum AccountType
    {
        Savings,
        Current,
        Corporate,
        Government
    }
}
