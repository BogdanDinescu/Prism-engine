using System.ComponentModel.DataAnnotations;

namespace Prism.Models
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,40}$", ErrorMessage = "Password must contain 1 lowercase, 1 digit, 1 uppercase. And length must be between 8 and 40 characters")]
        // password must contain at list 1 lowercase, 1 digit, 1 uppercase. And length between 8 and 40 characters
        public string NewPassword { get; set; }
    }
}
