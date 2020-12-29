using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prism.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public User User { get; set; }

        [Required]
        [RegularExpression(@"^([A-Za-z0-9\s]).{5,50}$", ErrorMessage = "You can't have special characters. And lenght must be between 5 and 50")]
        public string Caption { get; set; }

        [RegularExpression(@"^(http|https)://\S.{1,200}$", ErrorMessage = "This is not a link, or it's too long")]
        public string ImageUrl { get; set; }
    }
}
