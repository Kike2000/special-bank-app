using System.ComponentModel.DataAnnotations;

namespace SpecialBankAPI.Models
{
    public class UpdateAccountModel
    {
        [Required]
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin must not be more than 4 digits")]
        public string Pin { get; set; }
        [Required]
        [Compare("Pin", ErrorMessage = "Pin don't match")]
        public string ConfirmPin { get; set; }
        public string AccountNumberGenerated { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
