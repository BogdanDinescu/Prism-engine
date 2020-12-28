using System.ComponentModel.DataAnnotations;

namespace Prism.Models
{
    public class UserUpdate
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name could not be shorter than 2 characters long")]
        [MaxLength(40, ErrorMessage = "Name could not be longer than 40 characters long")]
        public string Name { get; set; }
    }
}
