using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prism.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [EmailValidator]
        [MaxLength(40, ErrorMessage = "Email too long")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,40}$", ErrorMessage = "Password must contain 1 lowercase, 1 digit, 1 uppercase. And length must be between 8 and 40 characters")]
        // password must contain at list 1 lowercase, 1 digit, 1 uppercase. And length between 8 and 40 characters
        public string Password { get; set; }

        public byte[] Salt { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Name could not be shorter than 2 characters long")]
        [MaxLength(40, ErrorMessage = "Name could not be longer than 40 characters long")]
        public string Name { get; set; }

        public string Role { get; set; } = "user";
    }
}