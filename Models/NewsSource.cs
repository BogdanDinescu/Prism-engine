using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prism.Models
{
    public class NewsSource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Link too long")]
        public string Link { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Name could not be shorter than 2 characters long")]
        [MaxLength(40, ErrorMessage = "Name could not be longer than 40 characters long")]
        public string Name { get; set; }

        public virtual ICollection<UserPreference> UserPreferences { get; set; }
    }
}
