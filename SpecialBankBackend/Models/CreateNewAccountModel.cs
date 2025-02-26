using System.ComponentModel.DataAnnotations;

namespace SpecialBankAPI.Models
{
    public class CreateNewAccountModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage ="Pin must not be more than 4 digits")]
        public string Pin {  get; set; }
        [Required]
        [Compare("Pin",ErrorMessage = "Pins don't match")]
        public string ConfirmPin { get; set; }
    }
}
